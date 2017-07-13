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
using xwcs.core.ui.db.fo;
using xwcs.core.db.binding;
using xwcs.core.db.fo;

namespace app.testing.laco
{
    public partial class TestValidation : DevExpress.XtraEditors.XtraForm
    {
        private FilterOptionsForm _fop = new FilterOptionsForm();

        public TestValidation()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            _fop.ShowDialog();
        }

        private void TestValidation_FormClosing(object sender, FormClosingEventArgs e)
        {
            _fop.Dispose();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            List<FilterOptions> data = new List<FilterOptions>();
            for (int i = 0; i < 10; ++i) data.Add(new FilterOptions());

            GridBindingSource _bs = new GridBindingSource();

            _bs.AttachToGrid(gridControl1);
            _bs.DataSource = data;
       
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            List<FilterOptions> data = new List<FilterOptions>();
            for (int i = 0; i < 10; ++i) data.Add(new FilterOptions());

            GridBindingSource _bs = new GridBindingSource();

            _bs.AttachToTree(treeList1);
            _bs.DataSource = data;
        }
    }
}