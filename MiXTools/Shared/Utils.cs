using MiXTools.View;
using Serilog;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace MiXTools.Shared
{
    /// <summary>
    /// <see cref="Utils"/> contains shared methods for MiXTools
    /// </summary>
    internal static class Utils
    {
        public static bool IsLogFormOpen { get; set; }
        /// <summary>
        /// Name of the Mi OSD Utility executable that is responsible for opening Mi Assistant or Mi Assistant's Microsoft Store page (if not installed)
        /// </summary>
        public const string ASSISTANT_BUTTON_FILENAME = "XaAppStore";
        public const string LNK = "lnk";
        public const string LNK_EXTENSION = "." + LNK;
        public const string SCR = "scr";
        public const string SCR_EXTENSION = "." + SCR;
        public const string EXE = "exe";
        public const string EXE_EXTENSION = "." + EXE;
        public const string APP_NAME = "MiXTools";
        public const string APP_PROCESS = APP_NAME + EXE_EXTENSION;
        public const string OWNER = "noelhorvath";
        public const string UAFL = "uafl";
        public const string URLANDFILELAUNCHER = "URLAndFileLauncher";
        public const string UAFL_CONFIG_FILE = UAFL;
        public const string UAFL_EXECUTABLE_FILE = UAFL + EXE_EXTENSION;
        public const string UAFL_VERIONS_STRING_SEPARATOR = "-";
        public const string UAFL_MIXTOOLS_VERSION_ID = "mxt";
        public const string OSD_LAUNCHER_FILE = "OSDLauncher" + EXE_EXTENSION;
        public const string TMP_FILE_NAME = "tmp";
        public const string CMD_EXECUTABLE = "cmd" + EXE_EXTENSION;
        public const string UAFL_MODE_RUNAS = "runas";
        public const string UAFL_MODE_OPEN = "open";
        public const string UAFL_MODE_EDIT = "edit";
        public const string UAFL_PROD_NAME = $"{URLANDFILELAUNCHER} for {APP_NAME}";
        public const string ASSISTANT_BUTTON_FILE = ASSISTANT_BUTTON_FILENAME + EXE_EXTENSION;
        public const string CMD_EXE = "cmd" + EXE_EXTENSION;
        /// <summary>
        /// Output template for logs
        /// </summary>
        public const string LOG_OUTPUT_TEMPLATE = "[{Level:u3}] {Timestamp:yyyy-MM-dd HH:mm:ss.fff}: {Message:lj}{NewLine}{Exception}";
        /// <summary>
        /// Generated log files' folder
        /// </summary>
        public const string LOG_FILES_PATH = "Logs";
        public const string GITHUB_URL = "https://github.com";
        public const string GITHUB_API_URL = "https://api.github.com";
        public const string GITHUB_REPO_URL = $"{GITHUB_URL}/{OWNER}/{APP_NAME}";
        public const string GITHUB_API_TAGS_PARAM = $"repos/{OWNER}/{URLANDFILELAUNCHER}/tags";
        public const string UAFL_VERSION_REGEX_PATTERN = $"^(\\d+\\{UAFL_VERIONS_STRING_SEPARATOR}({UAFL_MIXTOOLS_VERSION_ID}))$|^(\\d+\\.)+(\\d+\\{UAFL_VERIONS_STRING_SEPARATOR}({UAFL_MIXTOOLS_VERSION_ID}))$";
        /// <summary>
        /// Asynchronously runs a new process.
        /// </summary>
        /// <param name="executableName">Name of the executable file</param>
        /// <param name="command">Command which will be the input for the executable</param>
        /// <param name="arguments">Arguments for the executable</param>
        /// <returns>(<see cref="int"/>) Exit code of the process</returns>
        /// <exception cref="Exception">Thrown when the created process fails.</exception>
        internal static async Task<int> RunProcessAsync(string executableName, string command, string? arguments = null)
        {
            Log.Information("Starting new process...");
            Log.Debug($"Process details: file: \"{executableName}\", arguments: \"{arguments}\", command:{command}");
            using Process process = new();
            process.StartInfo.FileName = executableName;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Arguments = arguments;
            process.Start();
            Log.Debug($"{executableName} has been started");

            if (String.IsNullOrEmpty(command))
                return process.ExitCode;

            Log.Debug($"Executing \"{command}\" in \"{executableName}\"...");
            process.StandardInput.WriteLine(command);
            process.StandardInput.Flush();
            process.StandardInput.Close();
            await process.WaitForExitAsync();
            string cmdError = await process.StandardError.ReadToEndAsync();
            Log.Debug("cmd exit code: " + process.ExitCode);
            if (!String.IsNullOrEmpty(cmdError))
            {
                Log.Information("Process completion failed!");
                throw new Exception("cmd error: " + cmdError);
            }

            return process.ExitCode;
        }

        /// <summary>
        /// Replaces all occurances of (<see cref="char"/>) <paramref name="toReplace"/> with (<see cref="string"/>) <paramref name="replaceWith"/> in the reference <see cref="string"/> <paramref name="str"/>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="toReplace"></param>
        /// <param name="replaceWith"></param>
        public static void ReplaceCharWithStr(ref string str, char toReplace, string replaceWith)
        {
            string result = "";
            foreach (var c in str)
            {
                if (c == toReplace)
                    result += replaceWith;
                else
                    result += c;
            }
            str = result;
        }

        /// <summary>
        /// Terminates application in case of a critical error
        /// </summary>
        /// <param name="logMsg">Log message</param>
        /// <param name="boxMsg">MeassageBox text</param>
        /// <param name="exitCode">Application exit code</param>
        internal static void ErrorHandler(string logMsg, string boxMsg, string boxTitle, int exitCode, bool translate = true)
        {
            Log.Error(logMsg);
            var dialogResult = MessageBox.Show(translate ? GetTranslation(boxMsg) : boxMsg, translate ? GetTranslation(boxTitle) : boxTitle);
            Log.Debug("dialogResult: " + dialogResult.ToString());
            if (dialogResult == DialogResult.OK)
            {
                Environment.Exit(exitCode);
            }
        }

        public static string GetLogOutput()
        {
            return Directory.GetCurrentDirectory() + '\\' + LOG_FILES_PATH + '\\' + $"log_{DateTime.Now.Year}" + "_"
                + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString())
                + "_" + (DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString()) + ".txt";
        }

        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        public static void ChangeWindowDarkMode(IntPtr handle, bool isDarkMode)
        {
            _ = isDarkMode ? Utils.DwmSetWindowAttribute(handle, 20, new[] { 1 }, 4) : Utils.DwmSetWindowAttribute(handle, 20, new[] { 0 }, 4);
        }

        public static void OpenLogConsole()
        {
            Log.Information("Opening log console...");
            var loggingForm = new LogConsoleForm();
            loggingForm.FormClosing += LoggingForm_FormClosing;
            loggingForm.Show();
            IsLogFormOpen = true;
        }

        private static void LoggingForm_FormClosing(object? sender, FormClosingEventArgs? e)
        {
            if (sender != null)
            {
                Log.Information("Closing log console...");
                IsLogFormOpen = false;
            }
        }

        public static string GetTranslation(string key)
        {
            try
            {
                return LanguageManager.Instance.GetTranslation(key);
            }
            catch (Exception e)
            {
                Log.Error("GetTranslations error: " + e.Message);
                return e.Message;
            }
        }

        /// <summary>
        /// Returns the highest version out of a versions collection.
        /// </summary>
        /// <param name="versions">Collection of version strings</param>
        /// <returns>Newest version as a <see cref="string"/>/>.</returns>
        public static string GetNewestVersion(IEnumerable<string> versions, bool prefferLongerVersion = true, bool isCaseSensetive = false, string? versionRegexPattern = null)
        {
            string? latestVersion = null;
            foreach (var ver in versions)
            {
                string version = isCaseSensetive ? ver : ver.ToLower();
                Log.Debug("OSD version: " + version);
                if (!Regex.IsMatch(version, versionRegexPattern ?? "^\\d+(\\.\\d+[^\\.\\s]*)*$")) { continue; }
                if (latestVersion == null)
                    latestVersion = version;
                else
                {
                    if (version == latestVersion) { continue; } // skip if same
                    string[] latestVersionSplitted = latestVersion.Split('.');
                    string[] versionSplitted = version.Split('.');
                    for (int i = 0; i < latestVersionSplitted.Length; i++)
                    {
                        if (int.TryParse(latestVersionSplitted[i], out int lv) && int.TryParse(versionSplitted[i], out int v))
                        {
                            Log.Debug($"ver: {v}, latestVer: {lv} " + (lv < v));
                            if (lv < v)
                            {
                                latestVersion = version;
                                break;
                            }
                        }
                        else
                        {
                            char[] lvChars = latestVersionSplitted[i].ToCharArray();
                            char[] vChars = versionSplitted[i].ToCharArray();
                            int max = lvChars.Length < vChars.Length ? vChars.Length : lvChars.Length;
                            int min = lvChars.Length < vChars.Length ? lvChars.Length : vChars.Length;
                            for (int j = 0; j < max; j++)
                            {
                                if (min <= j)
                                {
                                    latestVersion = prefferLongerVersion ? (lvChars.Length < vChars.Length ? version : latestVersion) : (lvChars.Length < vChars.Length ? latestVersion : version);
                                    break;
                                }

                                Log.Debug($"vChar: {(int)vChars[j]} = {vChars[j]}, lvChar: {(int)vChars[j]} = {vChars[j]}, is newer: " + (lvChars[j] < vChars[j]));
                                if (lvChars[j] < vChars[j])
                                {
                                    latestVersion = version;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return latestVersion ?? "unknown";
        }

        // TODO: update messagebox text language
        /*
        public static void UpdateMessageBoxButtonsTextLanguage()
        {
            MessageBoxManager.OK = LocalResource.OK;
            MessageBoxManager.Cancel = LocalResource.Cancel;
            MessageBoxManager.Retry = LocalResource.Retry;
            MessageBoxManager.Ignore = LocalResource.Ignore;
            MessageBoxManager.Abort = LocalResource.Abort;
            MessageBoxManager.Yes = LocalResource.Yes;
            MessageBoxManager.No = LocalResource.No;
        }
        */
        public static string GetUAFLVersion()
        {
            return Properties.Settings.Default.UAFLVersion;
        }
        
        public static string GetUAFLDownloadURL()
        {
            return $"{GITHUB_URL}/{OWNER}/{URLANDFILELAUNCHER}/releases/download/v{GetUAFLVersion()}/{UAFL_EXECUTABLE_FILE}";
        }
    }

}
