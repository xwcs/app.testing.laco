using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using xwcs.core.evt;
using xwcs.indesign;
using xwcs.indesign.js;

// Cancellare il file C:\ProgramData\Adobe\InDesign\Version 8.0\it_IT\Scripting Support\8.0\Resources for Visual Basic.tlb
// Avviare InDesign come amministratore ricrea il file Resources for Visual Basic.tlb
// Aggiungere un riferimento COM a "Adobe Indesign CS6 Type Library"
//
// https://forums.adobe.com/thread/834780
// http://stackoverflow.com/questions/2483659/interop-type-cannot-be-embedded
// https://forums.adobe.com/thread/759540
// http://indesignsecrets.com/javascript-for-the-absolute-beginner.php#
// http://www.indiscripts.com/post/2010/02/how-to-create-your-own-indesign-menus
// http://www.indiscripts.com/post/2011/01/how-to-implement-a-basic-action-listener-in-indesign
// http://stackoverflow.com/questions/24433479/where-is-the-event-object-for-external-file-event-handlers
// https://msdn.microsoft.com/en-us/library/66ahbe6y%28v=vs.85%29.aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-1
// http://www.indiscripts.com/post/2013/05/indesign-scripting-forum-roundup-4#hd4sb3
// http://stackoverflow.com/questions/36845211/is-there-an-indesign-event-handler-for-when-new-pages-are-created

namespace app.testing.laco
{
   



    public partial class InDesignTestFormLaco : Form
    {
        lib.db.doc.niterdoc.NiterDocEntities _db;

        private lib.indesign.InDesignForBO _indesignFBO;
        public lib.indesign.InDesignForBO InDesignForBO
        {
            get
            {
                if (ReferenceEquals(null, _indesignFBO))
                {
                    _indesignFBO = new lib.indesign.InDesignForBO();
                }
                return _indesignFBO;
            }
        }


        public InDesignTestFormLaco()
        {
            xwcs.core.user.SecurityContext.getInstance().setUserProvider(new lib.core.user.BackOfficeUserProvider());
            var a = xwcs.core.user.SecurityContext.getInstance().CurrentUser;

            InitializeComponent();

            // invocation target for events
            SEventProxy.InvokeDelegate = this;
            // register user
            SIndesign.getInstance();    
            SIndesign.RegisterActiveUser(this);


            _db = new lib.db.doc.niterdoc.NiterDocEntities();

#if DEBUG_DB_LOG_ON
            _db.Database.Log = _logger.Debug;
#endif
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing )
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowDefaultWaitForm("Atendere prego", "Chiusura ...");

                // deregister user
                SIndesign.UnregisterActiveUser(this);

                // kill loggers
                xwcs.core.manager.SLogManager.getInstance().Dispose();
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseDefaultWaitForm();
                if(components != null)
                {
                    components.Dispose();
                }                
            }
            base.Dispose(disposing);
        }        

        private void button1_Click(object sender, EventArgs e)
        {

            // got events
            if (!SIndesign.HasAfterInit(InDesign_AfterInit))
            {
                SIndesign.AfterInit += InDesign_AfterInit;
            } 
            
            bool done = false;
            int panic = 10;
            while (!done && --panic > 0)
            try
            {
                // start all
                SIndesign.Start();
                    done = true;
            }catch(Exception ex)
                {
                    Console.Write(ex.Message);
                }


            // action
            if (!SIndesign.HasOnJsAction(SIndesign_OnJsAction))
            {
                SIndesign.OnJsAction += SIndesign_OnJsAction;
            }            
        }

        private void SIndesign_OnJsAction(object sender, OnMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
            // send some data back
            /*
            for(int i = 0; i < 10000; ++i)
            {
                sr[i] = new { id = i, waht = "data" + i };
            }    
            e.Result = new { id = "some", waht = "data", ar = sr };
            */

            lib.indesign.actions.ActionHandler ah = new lib.indesign.actions.ActionHandler();

            e.Result = ah.ExecuteAction(e.Message);
        }

        private void InDesign_AfterInit(object sender, EventArgs e)
        {
            string ver = SIndesign.CheckBridgeVersion();
            
            // load EGAF script
            string scr = new Composer().Compose("lib.indesign/run.js");
#if DEBUG_TRACE_LOG_ON
            _logger.Debug("Script: {0}", scr.Substring(0, 512));
#endif
            // here we need also 3 options for to have paths
            SIndesign.ExecScript(
                scr,
                new object[] {
                }
            );
            
            // do bindables
            EventBindable jeb = SIndesign.GetJsBindable(SIndesign.App.MenuActions[4]);

            jeb.AddEventHandler("beforeInvoke", indesign_beforeSave);

            // openFile
            SIndesign.FileManager.open("C:\\tmp\\Egafback\\Rtf\\432A2.rtf", new { id = "10", cod = "20" });
        }

        private void indesign_beforeSave(object sender, OnMessageEventArgs e)
        {
            Console.WriteLine("JsEvent");
            // read current story
            InDesign.Document d = SIndesign.App.ActiveDocument;
            if (d != null)
            {
                Console.WriteLine(d.Label);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            memoEdit1.Text = new Composer().Compose("id.js");

        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            memoEdit1.Text = new Composer().Compose("lib.indesign/run.js");
        }
        public class ReturnData
        {
            public int id { get; set; }
            public string OTA { get; set; }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            var a = _db.iter.Include("tipologie").AsNoTracking()
                .Where(i => i.tipologie.tipo == "normativa")
                .Select(i => new ReturnData() { id = i.id, OTA = i.OTA })
                .Take(10)
                .ToList();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            InDesignForBO.EditFile(_db.iter.Include("iter_in_xwbo_media.xwbo_media").AsNoTracking().Where(i => i.id == 126224).FirstOrDefault());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            InDesignForBO.EditFile(null, 126224);
        }
    }
    
}
