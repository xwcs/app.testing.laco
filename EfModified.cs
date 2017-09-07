using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using xwcs.core.evt;
using System.Diagnostics;

namespace app.testing.laco
{
    public partial class EfModified : DevExpress.XtraEditors.XtraForm
    {

        

        public EfModified()
        {
            xwcs.core.user.SecurityContext.getInstance().setUserProvider(new lib.core.user.BackOfficeUserProvider());
            SEventProxy.InvokeDelegate = this;
            InitializeComponent();
            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
            using(lib.db.doc.niterdoc.NiterDocEntities _ctx = new lib.db.doc.niterdoc.NiterDocEntities())
            {
                _ctx.Database.Log = Console.WriteLine;

                List<lib.db.doc.niterdoc.autori> l = _ctx.autori.Take(10).ToList();
                
                /*
                l[0].disabilitato = true;

               
                bool tt = l[0].disabilitato;
                bool v1 = l[0].GetIsReallyChanged();// l[0]);
                bool tt1 = l[0].disabilitato;
                var en = _ctx.Entry<lib.db.doc.niterdoc.autori>(l[0]);
                bool b1 = en.Property("disabilitato").IsModified;

                l[0].disabilitato = false;
                bool v2 = l[0].GetIsReallyChanged();
                en = _ctx.Entry<lib.db.doc.niterdoc.autori>(l[0]);
                bool b2 = en.Property("disabilitato").IsModified;
                bool b3 = Equals(en.OriginalValues["disabilitato"], en.CurrentValues["disabilitato"]);
                */


                Stopwatch sw = new Stopwatch();
                sw.Start();

               

                for (int i = 0; i < 100000; ++i)
                {
                    foreach(lib.db.doc.niterdoc.autori a in l)
                    {
                        a.disabilitato = !a.disabilitato;
                        bool t = a.GetIsReallyChanged();
                    }
                }

                sw.Stop();

                Console.WriteLine(sw.ElapsedMilliseconds);
               
            }
        }
    }
}