using lib.db.doc.niterdoc.massive;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xwcs.core.evt;

namespace app.testing.laco
{
    public partial class MassiveOperationsTest : Form
    {
        public MassiveOperationsTest()
        {
            xwcs.core.user.SecurityContext.getInstance().setUserProvider(new lib.core.user.BackOfficeUserProvider());
            SEventProxy.InvokeDelegate = this;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                lib.db.doc.niterdoc.NiterDocEntities db = new lib.db.doc.niterdoc.NiterDocEntities();
                MassiveOperations mo = new MassiveOperations(db);

                var openFileDialog1 = new OpenFileDialog()
                {
                    FileName = "Scegli indice di importazione",
                    Filter = "Indice (*.csv)|*.csv",
                    Title = "Apri indice di import",
                    Multiselect = false
                };

                // report errors
                List<RtfFileImportResult> errors = new List<RtfFileImportResult>();

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    splashScreenManager1.ShowWaitForm();
                    splashScreenManager1.SetWaitFormCaption("Atendere prego");

                    mo.ImportRTF(openFileDialog1.FileName, (s, msg) =>
                    {
                        if (s != null)
                        {
                            if (!s.Done)
                            {
                                // error
                                errors.Add(s);
                                memoEdit1.Text += "\r\nError: ---> " + s.Rtf;
                            } else
                            {
                                memoEdit1.Text += "\r\nOK: " + s.Rtf;
                            }
                            
                        }
                        else
                        {
                            memoEdit1.Text += "\r\n" + msg;
                        }


                        splashScreenManager1.SetWaitFormDescription(msg);

                    });

                    splashScreenManager1.CloseWaitForm();

                    if(errors.Count > 0)
                    {
                        // save errors 
                        System.IO.File.WriteAllText(string.Format(@"C:\Temp\MassiveImport-errors-{0}.txt", DateTime.Now.ToString("dd_mm_yyyy_hh_mm_ss")), string.Join("\r\n", errors.Select(er => er.Rtf.ToString() + "\r\n" + er.ex.ToString()).ToList()));
                    }
                }
            } catch(Exception ex)
            {
                splashScreenManager1.CloseWaitForm();
                MessageBox.Show(ex.ToString(), "Error");
            }
        }
    }
}
