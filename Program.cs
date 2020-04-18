using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app.testing.laco
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // some system init (dll 86/64 management)
            xwcs.core.Core.Init();

            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new MassiveOperationsTest());
            Application.Run(new GridlayoutConfigs());
            //Application.Run(new app.testing.laco.InDesignTestFormLaco());
            //Application.Run(new TestingVidgets());
            //Application.Run(new TestValidation());
            //Application.Run(new Http());
            //Application.Run(new EfModified());
            //Application.Run(new lib.db.states.pubblicazione.PubblicazioneSMTestForm());
        }
        
    }
}
