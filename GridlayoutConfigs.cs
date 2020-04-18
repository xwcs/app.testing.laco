using lib.db.titles.niterdoc.view;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xwcs.core.db.binding;

namespace app.testing.laco
{
    public partial class GridlayoutConfigs : Form
    {
        public GridlayoutConfigs()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GridLayoutsMan glm = new GridLayoutsMan();

            var l = glm.GetLayoutsByTypeAndPrefix(typeof(iter_titles), "someId");
            l.ForEach(ee => memoEdit1.Text += ee.CombinePath() + "\r\n");
        }
    }
}
