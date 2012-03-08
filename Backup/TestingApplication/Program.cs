using System;
using System.Collections.Generic;
using System.Windows.Forms;
using vbdetlevvb_engine;
namespace TestingApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Game g = new Game();
            g.Run(60.0);
        }
    }
}
