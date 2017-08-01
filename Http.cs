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
using System.Collections.Specialized;

namespace app.testing.laco
{
    public partial class Http : DevExpress.XtraEditors.XtraForm
    {
        public Http()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            xwcs.core.net.ExtraWayHTTPConnector cn = new xwcs.core.net.ExtraWayHTTPConnector();

            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("p1", "v1");
            xwcs.core.net.StoredProcResult spr = cn.CallStoredProc("$.tester.test", nvc);

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> kv  in spr)
            {
                sb.AppendFormat("{0}={1}\r\n", kv.Key, kv.Value);
            }

            memoEdit1.Text = sb.ToString();

        }
    }
}