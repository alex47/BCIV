using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCIV
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            switch(args.Length)
            {
                case 0:
                    Application.Run(new BCIV_form());
                    break;
                case 1:
                    Application.Run(new BCIV_form(args[0]));
                    break;
                default:
                    Application.Run(new BCIV_form(args));
                    break;
            }
        }
    }
}
