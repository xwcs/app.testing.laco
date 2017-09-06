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

namespace app.testing.laco
{
    public partial class EfModified : DevExpress.XtraEditors.XtraForm
    {

        private lib.db.doc.niterdoc.NiterDocEntities _ctx;

        public EfModified()
        {
            xwcs.core.user.SecurityContext.getInstance().setUserProvider(new lib.core.user.BackOfficeUserProvider());
            SEventProxy.InvokeDelegate = this;
            InitializeComponent();
            _ctx = new lib.db.doc.niterdoc.NiterDocEntities();
            _ctx.Database.Log = Console.WriteLine;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            List<lib.db.doc.niterdoc.autori> l = _ctx.autori.Take(10).ToList();

            l[0].disabilitato = true;
            var en = _ctx.Entry<lib.db.doc.niterdoc.autori>(l[0]);
            bool b1 = en.Property("disabilitato").IsModified;

            l[0].disabilitato = false;
            en = _ctx.Entry<lib.db.doc.niterdoc.autori>(l[0]);
            bool b2 = en.Property("disabilitato").IsModified;
            bool b3 = Equals(en.OriginalValues["disabilitato"], en.CurrentValues["disabilitato"]);

        }
    }
}