using DEiXTo.Services;
using System;
using System.Windows.Forms;

namespace DEiXTo
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

            IViewLoader viewLoader = new WindowsViewLoader();
            viewLoader.LoadMainView();
            Application.Run(viewLoader.LastLoadedView);
        }
    }
}
