using MiXTools.Shared;
using Serilog;
using System.Diagnostics;

namespace MiXTools.View
{
    internal static class Program
    {
        public const string FL_FILE_IS_MISSING_KEY = "FL_FILE_IS_MISSING";
        public const string FL_FILE_IS_MISSING_LOG = "Closing application, beacuse fl.exe is missing!";
        public const string APP_IS_ALREADY_RUNNING_KEY = "APP_IS_ALREADY_RUNNING";
        public const string APP_IS_ALREADY_RUNNING_LOG = "Application is already running!";
        public const string ERROR_TITLE_KEY = "ERROR";
        public const int FL_FILE_IS_MISSING_ERROR_CODE = 1;
        public const int APP_IS_ALREADY_RUNNING_ERROR_CODE = 2;
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]

        ///<summary>
        /// Application's entry point
        ///</summary>
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            // customize app configs
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetDefaultFont(new Font(new FontFamily("Segoe UI"), 8.25f));
            ApplicationConfiguration.Initialize();
            if (IsAppRunnningInAnotherProcess())
                Utils.ErrorHandler(APP_IS_ALREADY_RUNNING_LOG, APP_IS_ALREADY_RUNNING_KEY, ERROR_TITLE_KEY, APP_IS_ALREADY_RUNNING_ERROR_CODE);

            // create the main form
            MainForm mainForm = new();
            // add FormClosing event handler
            mainForm.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
            // launch main form
            Application.Run(mainForm);
        }

        /// <summary>
        /// FormClosing event handler for <see cref="MainForm"/>
        /// </summary>
        /// <param name="sender">Sender object or <see cref="null"/></param>
        /// <param name="e"><see cref="FormClosingEvent"/> data or <see cref="null"/></param>
        private static void MainForm_FormClosing(object? sender, FormClosingEventArgs? e)
        {
            Log.Information("Closing log console...");
            Log.CloseAndFlush();
            //?????
        }

        public static bool IsAppRunnningInAnotherProcess()
        {
            return Process.GetProcessesByName(Utils.APP_NAME).Length > 1;
        }
    }
}