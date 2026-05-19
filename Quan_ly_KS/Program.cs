using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quan_ly_KS
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string dataDir = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(Application.StartupPath, @"..\..\Data"));
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDir);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
