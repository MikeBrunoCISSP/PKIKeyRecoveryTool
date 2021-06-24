using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PKIKeyRecovery
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            RuntimeContext.Init();
            if (RuntimeContext.Conf.Valid)
            {
                Application.Run(new Form1());
            }
        }
    }
}
