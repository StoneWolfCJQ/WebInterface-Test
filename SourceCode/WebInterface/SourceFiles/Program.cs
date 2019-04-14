using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WebInterface
{
    static class Program
    {
        static IOInterface UI;
        static Thread th;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            UI = new IOInterface();
            th = new Thread(RunUI);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            Thread th1 = new Thread(SaveFile);
            th1.SetApartmentState(ApartmentState.STA);
            th1.IsBackground = true;
            //th1.Start();
        }

        [STAThread]
        static void RunUI()
        {
            //Application.Run(UI);
            try
            {
                Application.Run(UI);
            }
            catch (Exception e)
            {
                UI.SaveDefaultFile();
                throw new Exception(e.Message);
            }
        }

        [STAThread]
        static void SaveFile()
        {
            while (true)
            {
                Thread.Sleep(100);
                if (IODataCollection.SaveFile.SaveFile == 1)
                {
                    lock (IODataCollection.SaveFile)
                    {
                        try
                        {
                            UI.SaveDefaultFile();
                            IODataCollection.SaveFile.SaveFile = 0;
                        }
                        catch
                        {
                            IODataCollection.SaveFile.SaveFile = 0;
                        }
                    }
                }
            }
        }
    }
}
