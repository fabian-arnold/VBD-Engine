using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace vbdetlevvb_engine.Logging
{
    public class Logger
    {
        StreamWriter logwriter;
        private System.DateTime lasttime;
        Window window;
        public Logger(Window window)
        {

            Assembly assembly = Assembly.GetExecutingAssembly();
            string directoryName = Path.GetDirectoryName(assembly.Location);
            string logFileName = Path.Combine(directoryName, "log.txt");
            logwriter = new StreamWriter(logFileName);
            this.window = window;
            UpdateLasttime();
            
        }

        public void Update()
        {
            if ((System.DateTime.Now - lasttime) > TimeSpan.FromSeconds(59))
            {
                WriteLine("Time: " + System.DateTime.Now);
                UpdateLasttime();
            }
        }

        private void UpdateLasttime()
        {
            lasttime = System.DateTime.Now.AddSeconds(-System.DateTime.Now.Second);
            long lngSessMemory;
            lngSessMemory = Process.GetCurrentProcess().WorkingSet64;
            WriteLine("Performance Bericht: ");
            WriteLine("FPS: " + (window.UpdateFrequency/100));
            WriteLine("Memory in use: " + (Convert.ToDouble(lngSessMemory) / 1000 / 1000).ToString("0.##") );
        }

        

        public void Log(string tag, string txt)
        {
            
            string l = System.DateTime.Now.Second + "(" + tag + "): " + txt;
            WriteLine(l);
        }

        private void WriteLine(string l)
        {
            logwriter.WriteLine(l);
            Console.WriteLine(l);
        }
        public void Log(string txt)
        {
            Log("", txt);
        }


        public void Error(string p)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Beep();
            Log("Fatal Error", p);           
            Environment.Exit(0);
        }
    }
}
