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

namespace app.testing.laco
{
    public partial class MassiveOperationsTest : Form
    {
        public MassiveOperationsTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
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

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                splashScreenManager1.ShowWaitForm();
                splashScreenManager1.SetWaitFormCaption("Atendere prego");

                mo.ImportRTF(openFileDialog1.FileName, (s, msg) =>
                {
                    if(s != null)
                    {
                        memoEdit1.Text += "\r\n" + s.Rtf.fileName;
                    } else
                    {
                        memoEdit1.Text += "\r\n" + msg;
                    }
                    

                    splashScreenManager1.SetWaitFormDescription(msg);

                });

                splashScreenManager1.CloseWaitForm();
            }
        }
    }
}
