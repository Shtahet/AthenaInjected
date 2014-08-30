using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Athena.QuickInjector
{
    class Program
    {
        public static readonly Type InjectedDomainManagerEntryPoint = typeof(Athena.DomainManager.Startup);

        static BackgroundWorker bgwInjector;

        static void Main(string[] args)
        {
            bgwInjector = new BackgroundWorker { WorkerReportsProgress = true };
            bgwInjector.ProgressChanged += bgwInjector_ProgressChanged;
            bgwInjector.DoWork += bgwInjector_DoWork;

            bgwInjector.RunWorkerAsync();

            Console.WriteLine("Please wait while I attemp injection!");

            Console.ReadLine();
        }

        static void bgwInjector_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!Process.GetProcessesByName("wowb").Any())
            {
                Console.WriteLine("QuickInjector could not find any wow processes");
                return;
            }

            var injector = new DotNetInjector(Process.GetProcessesByName("wowb").First());
            injector.InjectAndForget(InjectedDomainManagerEntryPoint, Application.StartupPath + "\\Athena.Core.dll");

            bgwInjector.ReportProgress(50, "Injecting...");

            while (!injector.Injected)
            {

            }

            bgwInjector.ReportProgress(100, "Injected!");
        }

        static void bgwInjector_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 50)
            {
                Console.WriteLine(e.UserState.ToString());
                //button1.Text = e.UserState.ToString();
            }

            if (e.ProgressPercentage == 100)
            {
                Console.WriteLine("We should now be injected!");

                //Application.Exit();
            }
        }

    }
}
