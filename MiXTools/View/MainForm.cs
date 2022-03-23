using MiXTools.Controller;
using MiXTools.DAO;
using MiXTools.Model;
using MiXTools.Shared;
using Serilog;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace MiXTools.View
{
    public partial class MainForm : Form, IViewDarkMode, IViewLanguageManager
    {
        private readonly OSDController osdController;
        // for notification tray
        //private readonly NotifyIcon notifyIcon1;
        //private readonly ContextMenuStrip contextMenu;
        //private readonly ToolStripMenuItem menuItem1;
        private readonly LanguageManager languageManager;
        public readonly string ERROR_TITLE_KEY = "ERROR";
        public readonly string UAFL_DOWNLOAD_FAILED_KEY = "UAFL_DOWNLOAD_FAILED";
        public readonly string UAFL_MODE_CANNOT_BE_CHANGED_KEY = "UAFL_MODE_CANNOT_BE_CHANGED";
        public readonly int UAFL_DOWNLOAD_FAILED_ERROR_CODE = 31;
        public bool DarkMode { get; set; }
        public Color LabelFontColor { get; set; }
        public Color LabelBackgroundColor { get; set; }
        public Color BtnFontColor { get; set; }
        public Color BtnBorderColor { get; set; }
        public Color BtnBackgroundColor { get; set; }
        public FlatStyle BtnStyle { get; set; }
        public Color FormBackgroundColor { get; set; }
        public Color BtnMouseEnterFontColor { get; set; }
        public Color BtnMouseEnterBorderColor { get; set; }
        public Color BtnMouseEnterBackgroundColor { get; set; }
        public Color BtnDisabledFontColor { get; set; }
        public Color BtnDisabledBorderColor { get; set; }
        public Color BtnDisabledBackgroundColor { get; set; }
        public int BtnBorderSize { get; set; }
        public FontFamily DefaultFontFamily { get; set; }
        public float ExtraSmallFontSize { get; set; }
        public float SmallFontSize { get; set; }
        public float NormalFontSize { get; set; }
        public float BigFontSize { get; set; }
        public GraphicsUnit FontUnit { get; set; }
        public FontStyle DefaultFontStyle { get; set; }
        public bool OpenLogConsoleOnStartup { get; set; }
        public FileStream? UAFLFileStream { get; set; }
        public FileStream? UAFLConfigFileStream { get; set; }
        public bool IsSettingsFormOpen { get; set; }
        public MainForm() : this(LanguageManager.Instance) { }
        public MainForm(LanguageManager languageManager)
        {
            OpenLogConsoleOnStartup = Properties.Settings.Default.OpenLogConsoleOnStartup;
            if (OpenLogConsoleOnStartup) { Utils.OpenLogConsole(); }
            // disable resize
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            UAFLFileStream = null;
            UAFLConfigFileStream = null;

            DefaultFontFamily = FontFamily.GenericSansSerif;
            this.languageManager = languageManager;
            osdController = new OSDController(new OSDDAO());

            InitializeComponent();
            InitTheme();

            mainProgressBar.Visible = false;

            SettingsManager.AddSettingsPropertyChangeEventHandler(Settings_PropertyChanged);
            UpdateTextLanguage();

            //URLAndFileLauncher specific initializers
            CheckForNewOSDVersion();
            _ = SetupURLAndFileLauncher();

            /*
            // init tray and items
            contextMenu = new ContextMenuStrip();
            menuItem1 = new ToolStripMenuItem();
            contextMenu.Items.Add(menuItem1);

            menuItem1.Text = "Close";
            menuItem1.Click += new EventHandler(MenuItem1_Click);
            components = new Container();
            notifyIcon1 = new NotifyIcon(components)
            {
                Icon = new Icon(Properties.Resources.appicon, new Size(32, 32)),
                ContextMenuStrip = contextMenu,
                // icon hover text
                Text = languageManager.GetTranslation("MIXTOOLS"),
                Visible = true
            };

            notifyIcon1.DoubleClick += new EventHandler(NotifyIcon1_DoubleClick);
            */

            //set link clicked event handler
            githubLinkLabel.LinkClicked += GitHubLinkLabel_LinkClicked;
        }

        public void InitTheme()
        {
            DarkMode = Properties.Settings.Default.DarkMode;
            SetStaticThemeProperties();
            ChangeDarkMode();
            InitializeButtonThemeChangingEventHandlers();
        }

        public void UpdateThemeProperties()
        {
            LabelFontColor = DarkMode ? Color.White : Color.Black;
            BtnFontColor = DarkMode ? Color.Black : Color.Black;
            BtnBackgroundColor = DarkMode ? Color.DarkOrange : Color.Orange;
            BtnBorderColor = DarkMode ? Color.DarkOrange : Color.Orange;
            BtnMouseEnterBackgroundColor = DarkMode ? Color.Orange : Color.DarkOrange;
            BtnMouseEnterFontColor = Color.White;
            BtnMouseEnterBorderColor = DarkMode ? Color.Black : Color.White;
            FormBackgroundColor = DarkMode ? Color.Black : Color.White;
            BtnDisabledBackgroundColor = DarkMode ? Color.LightGray : Color.DarkGray;
            BtnDisabledFontColor = DarkMode ? Color.Black : Color.White;
            BtnDisabledBorderColor = DarkMode ? Color.LightGray : Color.DarkGray;
        }


        public void SetStaticThemeProperties()
        {
            LabelBackgroundColor = Color.Transparent;
            BtnStyle = FlatStyle.Flat;
            BtnBorderSize = 2;
            try
            {
                DefaultFontFamily = new FontFamily(IViewDarkMode.FONT_SEGOE_UI);
            }
            catch
            {
                Log.Error("Can't set font to Segoi UI");
                Log.Error("Changing font to GenericSansSerif");
            }
            FontUnit = GraphicsUnit.Pixel;
            DefaultFontStyle = FontStyle.Bold;
            ExtraSmallFontSize = 20;
            SmallFontSize = 25;
            NormalFontSize = 30;
            BigFontSize = 35;
        }

        /*
        private void NotifyIcon1_DoubleClick(object? sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }

            Activate();
        }
        

        private void MenuItem1_Click(object? sender, EventArgs e)
        {
            Close();
        }
        */

        private void UpdateOSDTexts()
        {
            try
            {
                Log.Information("Updating osd texts...");
                OSD osd = osdController.GetOSD();
                osdFolderText.Text = osd.Path;
                osdVersionText.Text = osd.Version;
                osdCurrentURLOrFilePathForAssistantButtonText.Text = osd.CurrentURLOrFilePathForAssistantButton;
                osdUAFLModeText.Text = languageManager.GetTranslation(osd.UAFLMode.ToUpper());
                Log.Debug("UpdateOSDTexts OSD info: " + osd.ToString());
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                Log.Error(e.StackTrace);
            }
        }

        private async void SelectOSDFolder(object sender, EventArgs e)
        {
            try
            {
                StartLoading();
                Log.Information("Opening folder browser...");
                FolderBrowserDialog folderBrowserDialog = new();
                DialogResult dialogResult = folderBrowserDialog.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    string path = folderBrowserDialog.SelectedPath;
                    Log.Information($"Selected folder: {path}");
                    if (Directory.GetFiles(path).ToList().Where(file => file.Contains(Utils.OSD_LAUNCHER_FILE)).ToArray().Length != 0)
                    {
                        Log.Debug($"{Utils.OSD_LAUNCHER_FILE} found in {path}");
                        string? latestVersion = OSDController.GetNewestOSDVersionFromDirs(path);
                        Log.Information($"Current OSD version is {latestVersion}");

                        try
                        {
                            OSD currentOSD = osdController.GetOSD();
                            Log.Debug($"Current OSD info: {currentOSD}");
                            if (currentOSD.Path == null || currentOSD.Path == "-")
                            {
                                Log.Information("Saving OSD information...");
                                osdController.AddOSD(new(path, latestVersion, Utils.GITHUB_REPO_URL, Utils.UAFL_MODE_OPEN));
                            }
                            else
                            {
                                Log.Information("Updating OSD information...");
                                osdController.UpdateOSD(new() { Path = path, Version = latestVersion });
                            }

                            OSD osd = osdController.GetOSD();
                            Log.Debug($"Loaded OSD information: path: {osd.Path}, version: {osd.Version}");
                            UpdateOSDTexts();
                            await SetupURLAndFileLauncher();
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.Message);
                            Log.Error(ex.StackTrace);
                        }
                    }
                    else
                    {
                        Log.Error($"No {Utils.OSD_LAUNCHER_FILE} found in the selected folder!");
                        MessageBox.Show(languageManager.GetTranslation("OSDLAUNCHER_EXE_NOT_FOUND"), languageManager.GetTranslation("ERROR"));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
            } 
            finally
            {

            StopLoading();
            }

        }

        private async void ChangeXaAppStoreButton_Click(object sender, EventArgs e)
        {
            try
            {
                var osd = osdController.GetOSD();
                if (osd.IsEmpty())
                {
                    Log.Error("OSD Path is not set");
                    MessageBox.Show(languageManager.GetTranslation("OSD_PATH_IS_NOT_SET"), languageManager.GetTranslation("ERROR"));
                }
                else
                {
                    StartLoading();
                    OpenFileDialog openFileDialog = new();
                    Log.Information("Opening file dialog...");
                    openFileDialog.Title = "Select an application or a shortcut";
                    //openFileDialog.ShowReadOnly = true;
                    openFileDialog.Filter = "All files (*.*)|*.*";
                    openFileDialog.DereferenceLinks = false;
                    DialogResult dialogResult = openFileDialog.ShowDialog();

                    if (dialogResult == DialogResult.OK)
                    {
                        // runas only works with .lnk or .exe
                        if (openFileDialog.FileName.EndsWith(".lnk") || openFileDialog.FileName.EndsWith(".exe"))
                        {
                            var msgBoxDialogResult = MessageBox.Show(
                                languageManager.GetTranslation("SELECT_LAUNCH_MODE"),
                                languageManager.GetTranslation("SELECT_LAUNCH_MODE_TITLE"),
                                MessageBoxButtons.YesNo
                            );
                            await ChangeXaAppStore(msgBoxDialogResult == DialogResult.Yes, openFileDialog.FileName);
                        }
                        else
                            await ChangeXaAppStore(false, openFileDialog.FileName);
                    }
                }
            }
            catch (UnauthorizedAccessException ue)
            {
                Log.Warning("No admin priviliges");
                Log.Error(ue.Message);
                Log.Error(ue.StackTrace);
                MessageBox.Show(languageManager.GetTranslation("UNAUTHORIZED_ACCESS"), languageManager.GetTranslation("ERROR"));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
            }
            finally 
            { 
                StopLoading(); 
            }
        }

        private async Task ChangeXaAppStore(bool createWithRunAsAdmin, string file)
        {
            Log.Information($"Selected file: {file}");
            OSD osd = osdController.GetOSD();
            string uafl_mode = createWithRunAsAdmin ? Utils.UAFL_MODE_RUNAS : Utils.UAFL_MODE_OPEN;
            var uafl_config = new List<string>() { file, uafl_mode };
            await IOHelper.WriteDataToFileAsync(Application.UserAppDataPath + '\\' + Utils.UAFL_CONFIG_FILE, uafl_config, true);
            osdController.UpdateOSD(osd.Path, osd.Version, file, uafl_mode);
            UpdateOSDTexts();
        }

        private void OpenLogConsoleButton_Click(object sender, EventArgs e)
        {
            Utils.OpenLogConsole();
        }



        public void StartLoading()
        {
            mainProgressBar.Visible = true;
            Cursor = Cursors.WaitCursor;
            SetButtons(false);
        }

        public void StopLoading()
        {
            mainProgressBar.Visible = false;
            Cursor = Cursors.Default;
            SetButtons(true);
        }

        private void SetButtons(bool isEnabled)
        {
            osdSelectButton.Enabled = isEnabled;
            osdShowScreenSaverButton.Enabled = isEnabled;
            osdChangeXaAppStoreButton.Enabled = isEnabled;
            osdChangeUAFLModeButton.Enabled = isEnabled;
            if (!IsSettingsFormOpen)
                settingsButton.Enabled = isEnabled;
        }

        private async void ShowScreenSaverButton_Click(object sender, EventArgs e)
        {
            try
            {
                OSD osd = osdController.GetOSD();
                string screenSaverFile = Directory.GetFiles(osd.GetCurrentVersionPath()).Where(f => f.EndsWith(Utils.SCR_EXTENSION)).First();
                Log.Debug("Mi Screen Saver file: " + screenSaverFile);
                string command = $"\"{screenSaverFile}\" /s";
                int exitCode = await Utils.RunProcessAsync(Utils.CMD_EXECUTABLE, command);
            }
            catch (OSDException)
            {
                Log.Warning("Can't access Mi Screen Saver, because Mi OSD Utility path is not set");
                MessageBox.Show(languageManager.GetTranslation("CANNOT_ACCESS_MI_SCREEN_SAVER"), languageManager.GetTranslation("ERROR"));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
            }
        }

        public void ChangeButtonTheme_MouseEnter(object? sender, EventArgs args)
        {
            if (sender != null)
            {
                Button btn = (Button)sender;
                btn.SetTheme(BtnMouseEnterFontColor, BtnMouseEnterBackgroundColor, BtnMouseEnterBorderColor, BtnBorderSize);
            }

        }

        public void ChangeButtonTheme_MouseLeave(object? sender, EventArgs args)
        {
            if (sender != null)
            {
                Button btn = (Button)sender;
                btn.SetTheme(BtnFontColor, BtnBackgroundColor, BtnBorderColor, BtnBorderSize);
            }
        }

        public void ChangeButtonTheme_EnabledChanged(object? sender, EventArgs args)
        {
            if (sender != null)
            {
                Button btn = (Button)sender;
                if (btn.Enabled)
                    btn.SetTheme(BtnFontColor, BtnBackgroundColor, BtnBorderColor, BtnBorderSize);
                else
                    btn.SetTheme(BtnDisabledFontColor, BtnDisabledBackgroundColor, BtnDisabledBorderColor, BtnBorderSize);

            }
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            OpenSettings();
        }

        private void OpenSettings()
        {
            var form = new SettingsForm();
            form.FormClosing += SettingsForm_FormClosing;
            form.PropertyChanged += SettingsForm_PropertyChanged;
            form.Show();
            IsSettingsFormOpen = true;
            settingsButton.Enabled = false;
        }

        private void SettingsForm_FormClosing(object? sender, FormClosingEventArgs? e)
        {
            if (sender != null)
            {
                Log.Information("Closing settings...");
                IsSettingsFormOpen = false;
                settingsButton.Enabled = true;
            }
        }

        private void SettingsForm_PropertyChanged(object? sender, PropertyChangedEventArgs? args)
        {
            if (sender != null)
            {
                SettingsForm form = (SettingsForm)sender;
                Log.Debug("SettingsForm PropertyChanged: " + args?.PropertyName);
                if (args != null)
                {
                    if (args.PropertyName != null && args.PropertyName.Equals("IsDataReset") && form.IsDataReset)
                    {
                        Log.Debug("SettingsForm IsDataReset changed to: " + form.IsDataReset);
                        form.IsDataReset = false;
                        UpdateOSDTexts();
                    }

                }
            }
        }

        public void ChangeDarkMode()
        {
            Utils.ChangeWindowDarkMode(Handle, DarkMode);
            UpdateThemeProperties();
            this.BackColor = FormBackgroundColor;
            osdTitle.ForeColor = LabelFontColor;
            // label theme
            osdFolderText.SetTheme(LabelFontColor, LabelBackgroundColor);
            osdFolderLabel.SetTheme(LabelFontColor, LabelBackgroundColor);
            osdVersionLabel.SetTheme(LabelFontColor, LabelBackgroundColor);
            osdVersionText.SetTheme(LabelFontColor, LabelBackgroundColor);
            osdCurrentURLOrFilePathForAssistantButtonLabel.SetTheme(LabelFontColor, LabelBackgroundColor);
            osdCurrentURLOrFilePathForAssistantButtonText.SetTheme(LabelFontColor, LabelBackgroundColor);
            osdUAFLModeLabel.SetTheme(LabelFontColor, LabelBackgroundColor);
            osdUAFLModeText.SetTheme(LabelFontColor, LabelBackgroundColor);
            githubLinkLabel.SetTheme(LabelFontColor, LabelBackgroundColor);
            // set button theme
            osdSelectButton.SetTheme(BtnFontColor, BtnBackgroundColor, BtnBorderColor, BtnBorderSize, BtnStyle);
            osdChangeXaAppStoreButton.SetTheme(BtnFontColor, BtnBackgroundColor, BtnBorderColor, BtnBorderSize, BtnStyle);
            osdShowScreenSaverButton.SetTheme(BtnFontColor, BtnBackgroundColor, BtnBorderColor, BtnBorderSize, BtnStyle);
            settingsButton.SetTheme(BtnFontColor, BtnBackgroundColor, BtnBorderColor, BtnBorderSize, BtnStyle);
            osdChangeUAFLModeButton.SetTheme(BtnFontColor, BtnBackgroundColor, BtnBorderColor, BtnBorderSize, BtnStyle);
            // set button font
            osdSelectButton.SetFont(NormalFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            osdChangeXaAppStoreButton.SetFont(NormalFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            osdShowScreenSaverButton.SetFont(NormalFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            settingsButton.SetFont(NormalFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            osdChangeUAFLModeButton.SetFont(NormalFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            // set label font
            osdTitle.SetFont(BigFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            osdFolderText.SetFont(SmallFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            osdFolderLabel.SetFont(NormalFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            osdVersionLabel.SetFont(NormalFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            osdVersionText.SetFont(NormalFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            osdCurrentURLOrFilePathForAssistantButtonLabel.SetFont(NormalFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            osdCurrentURLOrFilePathForAssistantButtonText.SetFont(SmallFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            osdUAFLModeLabel.SetFont(NormalFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            osdUAFLModeText.SetFont(NormalFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            githubLinkLabel.SetFont(NormalFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
        }

        public void InitializeButtonThemeChangingEventHandlers()
        {
            osdSelectButton.AddEventHandlers(
                ChangeButtonTheme_MouseEnter,
                ChangeButtonTheme_MouseLeave,
                ChangeButtonTheme_EnabledChanged
            );
            osdChangeXaAppStoreButton.AddEventHandlers(
                ChangeButtonTheme_MouseEnter,
                ChangeButtonTheme_MouseLeave,
                ChangeButtonTheme_EnabledChanged
            );
            osdShowScreenSaverButton.AddEventHandlers(
                ChangeButtonTheme_MouseEnter,
                ChangeButtonTheme_MouseLeave,
                ChangeButtonTheme_EnabledChanged
            );
            settingsButton.AddEventHandlers(
                ChangeButtonTheme_MouseEnter,
                ChangeButtonTheme_MouseLeave,
                ChangeButtonTheme_EnabledChanged
            );
            osdChangeUAFLModeButton.AddEventHandlers(
                ChangeButtonTheme_MouseEnter,
                ChangeButtonTheme_MouseLeave,
                ChangeButtonTheme_EnabledChanged
            );
        }

        public void UpdateTextLanguage()
        {
            try
            {
                Log.Debug($"Changing MainForm display language to {languageManager.CurrentLanguage}...");
                osdTitle.Text = languageManager.GetTranslation("OSD_SETTINGS");
                osdFolderLabel.Text = languageManager.GetTranslation("FOLDER_LOCATION");
                osdVersionLabel.Text = languageManager.GetTranslation("VERSION");
                osdCurrentURLOrFilePathForAssistantButtonLabel.Text = languageManager.GetTranslation("ASSISTANT_BUTTON_URL_OR_FILE");
                osdUAFLModeLabel.Text = languageManager.GetTranslation("UAFL_MODE");
                osdSelectButton.Text = languageManager.GetTranslation("CHANGE");
                osdChangeXaAppStoreButton.Text = languageManager.GetTranslation("CHANGE");
                osdChangeUAFLModeButton.Text = languageManager.GetTranslation("CHANGE");
                osdShowScreenSaverButton.Text = languageManager.GetTranslation("SHOW_MI_SCREEN_SAVER");
                settingsButton.Text = languageManager.GetTranslation("SETTINGS");
                githubLinkLabel.Text = languageManager.GetTranslation("VISIT_GITHUB");
                this.Text = languageManager.GetTranslation("MIXTOOLS");
                UpdateOSDTexts();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        public void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs args)
        {
            Log.Debug($"Settings property {args.PropertyName} changed in MainForm");
            switch (args.PropertyName)
            {
                case "DarkMode":
                    DarkMode = Properties.Settings.Default.DarkMode;
                    ChangeDarkMode();
                    break;
                case "CurrentLanguage":
                    UpdateTextLanguage();
                    break;
                default:
                    break;
            }
        }
        public void Unsubscribe()
        {
            SettingsManager.RemoveSettingsPropertyChangeEventHandler(Settings_PropertyChanged);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Unsubscribe();
                CloseFileResources();
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        private async Task SetupURLAndFileLauncher()
        {
            try
            {
                StartLoading();
                OSD osd = osdController.GetOSD();
                // only check for latest UAFL version if the OSD path is specified
                if (osd.Path == "-")
                {
                    Log.Debug("URLAndFileLauncher setup stopped, because OSD path is undefined!");
                    StopLoading();
                    return;
                }

                Log.Information("Checking for URLAndFileLauncher update...");
                string uaflLatestVersion = Utils.GetUAFLVersion();
                string uaflSrc = Directory.GetCurrentDirectory() + '\\' + Utils.UAFL_EXECUTABLE_FILE;
                Log.Debug("SetupURLAndFileLauncher -> uaflSrc: " + uaflSrc);
                // check for URLAndFileLauncher update
                try
                {
                    List<string> versions = (await HttpHelper.GetDataAsync<Tag>(Utils.GITHUB_API_URL, Utils.GITHUB_API_TAGS_PARAM)).Select(tag => tag.Name).Where(version => Regex.IsMatch(version, Utils.UAFL_VERSION_REGEX_PATTERN)).ToList();
                    string latestVerion = Utils.GetNewestVersion(versions);
                    if (!latestVerion.Equals("unknown"))
                    {
                        uaflLatestVersion = latestVerion;
                    }
                    Log.Information($"Latest URLAndFileLauncher verion is {uaflLatestVersion}");
                }
                catch (Exception ex)
                {
                    Log.Error("Get UAFL latest version tag error: " + ex.Message);
                }

                string uaflDest = osd.GetCurrentVersionPath() + '\\' + Utils.ASSISTANT_BUTTON_FILE;
                Log.Debug("SetupURLAndFileLauncher -> uaflDest: " + uaflDest);
                
                var srcProdVersion = File.Exists(uaflSrc) ? FileVersionInfo.GetVersionInfo(uaflSrc).ProductVersion : null;
                var srcProdName = File.Exists(uaflSrc) ? FileVersionInfo.GetVersionInfo(uaflSrc).ProductName : null;
                Log.Debug("srcProdName: " + srcProdName);
                Log.Information($"Current version of URLAndFileLauncher: {srcProdVersion}");
                Log.Information($"Latest version of URLAndFileLauncher: {uaflLatestVersion}");

                if (string.IsNullOrEmpty(srcProdVersion) || !srcProdVersion.Equals(uaflLatestVersion)
                    || string.IsNullOrEmpty(srcProdName) || !srcProdName.Equals(Utils.UAFL_PROD_NAME))
                {
                    try
                    {
                        Log.Information($"Updating URLAndFileLauncher to the newest version ({uaflLatestVersion})...");
                        await HttpHelper.DownloadFileAsync(Utils.GetUAFLDownloadURL(), uaflSrc);
                        _ = IOHelper.CopyFile(uaflSrc, uaflDest, true, false);
                        
                    }
                    catch (HttpRequestException hre)
                    {
                        Utils.ErrorHandler($"DownloadFileAsync error code: {hre.Message}", UAFL_DOWNLOAD_FAILED_KEY, ERROR_TITLE_KEY, UAFL_DOWNLOAD_FAILED_ERROR_CODE, true);
                    }
                    
                    // lock uafl file
                    try
                    {
                        if (File.Exists(uaflSrc))
                        {
                            if (UAFLFileStream != null)
                                UAFLFileStream.Close();
                            
                            UAFLFileStream = IOHelper.LockFile(uaflSrc, FileMode.Open, FileAccess.Read, FileShare.Read);
                        }
                            
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Lock uafl file error: " + ex.Message);
                    }
                }
                else
                {
                    // TODO: fix uafl config file not getting created...

                    var destVersion = File.Exists(uaflDest) ? FileVersionInfo.GetVersionInfo(uaflDest).ProductVersion : null;
                    var destProdName = File.Exists(uaflDest) ? FileVersionInfo.GetVersionInfo(uaflDest).ProductName : null;
                    string uaflConfigFile = Application.UserAppDataPath + '\\' + Utils.UAFL;

                    if (string.IsNullOrEmpty(destVersion) || !destVersion.Equals(uaflLatestVersion)
                       || string.IsNullOrEmpty(destProdName) || !destProdName.Equals(Utils.UAFL_PROD_NAME)
                       || !File.Exists(uaflConfigFile))
                    {
                        Log.Information("Updating URLAndFileLauncher in Mi OSD Utility folder...");
                        bool res = false;
                        if (res = IOHelper.CopyFile(uaflSrc, uaflDest, true, false))
                        {
                            Log.Information("Update status: " + (res ? "success" : "failed"));
                            string[] uaflConfigFileData = new string[] { osd.CurrentURLOrFilePathForAssistantButton, osd.UAFLMode };
                            if (File.Exists(uaflConfigFile))
                            {
                                IEnumerable<string> uaflData = await IOHelper.ReadDataFromFileAsync(uaflConfigFile, Encoding.UTF8);
                  
                                if (!uaflData.Contains(osd.CurrentURLOrFilePathForAssistantButton) || !uaflData.Contains(osd.UAFLMode))
                                    await IOHelper.WriteDataToFileAsync(uaflConfigFile, uaflConfigFileData, true);
                            }
                            else
                            {
                                Log.Debug($"SetupURLAndFileLauncher TryMovingFile res: {res}");
                                await IOHelper.WriteDataToFileAsync(uaflConfigFile, uaflConfigFileData);
                            }

                            // lock uafl config file
                            try
                            {
                                if (File.Exists(uaflConfigFile))
                                {
                                    if (UAFLConfigFileStream != null)
                                        UAFLConfigFileStream.Close();
                                    
                                    UAFLConfigFileStream = IOHelper.LockFile(uaflConfigFile, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Lock uafl config file error: " + ex.Message);
                            }
                        }
                    }
                    else
                        Log.Information("URLAndFileLauncher is up-to-date!");
                }
                StopLoading();
            }
            catch (Exception ex)
            {
                Log.Error("SetupURLAndFileLauncher error: " + ex.Message);
                Log.Error(ex.StackTrace);
            }
        }

        private void CheckForNewOSDVersion()
        {
            Log.Information("Checking for new OSD version on startup...");
            OSD osd = osdController.GetOSD();
            if (osd.Path == "-") {
                Log.Information("Check stopped, beacuse OSD path is not defined!");
                return;
            }
            string? version = OSDController.GetNewestOSDVersionFromDirs(osd.Path);
            if (version != null && osd.Version != version)
            {
                Log.Information("New Mi OSD Utility version has been detected!");
                Log.Information("Updating OSD version...");
                osdController.UpdateOSDVersion(version);
                UpdateOSDTexts();
            }
        }

        private async void ChangeUAFLMode_Click(object sender, EventArgs e)
        {
            OSD osd = osdController.GetOSD();
            if (!osd.CurrentURLOrFilePathForAssistantButton.Equals("-"))
            {
                if (osd.CurrentURLOrFilePathForAssistantButton.EndsWith(".exe") || osd.CurrentURLOrFilePathForAssistantButton.EndsWith(".lnk"))
                {
                    var uafl_config = new List<string>() { osd.CurrentURLOrFilePathForAssistantButton };
                    var msgBoxDialogResult = MessageBox.Show(
                        languageManager.GetTranslation("SELECT_LAUNCH_MODE"),
                        languageManager.GetTranslation("SELECT_LAUNCH_MODE_TITLE"),
                        MessageBoxButtons.YesNo
                    );
                    if (msgBoxDialogResult == DialogResult.Yes)
                    {
                        if (osd.UAFLMode != Utils.UAFL_MODE_RUNAS)
                        {
                            osdController.UpdateOSDFLMode(Utils.UAFL_MODE_RUNAS);
                            uafl_config.Add(Utils.UAFL_MODE_RUNAS);
                            await IOHelper.WriteDataToFileAsync(Application.UserAppDataPath + '\\' + Utils.UAFL, uafl_config, true);
                            UpdateOSDTexts();
                        }
                    }
                    else
                    {
                        if (osd.UAFLMode != Utils.UAFL_MODE_OPEN)
                        {
                            osdController.UpdateOSDFLMode(Utils.UAFL_MODE_OPEN);
                            uafl_config.Add(Utils.UAFL_MODE_OPEN);
                            await IOHelper.WriteDataToFileAsync(Application.UserAppDataPath + '\\' + Utils.UAFL, uafl_config, true);
                            UpdateOSDTexts();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(languageManager.GetTranslation(UAFL_MODE_CANNOT_BE_CHANGED_KEY), "ERROR");
                }
            }

        }

        private void GitHubLinkLabel_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs args)
        {
            Log.Debug("Opening Project's GitHub link...");
            try
            {
                Process.Start(new ProcessStartInfo { FileName = Utils.GITHUB_REPO_URL, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
            }
        }

        private void CloseFileResources()
        {
            if (UAFLFileStream != null)
                UAFLFileStream.Close();

            if (UAFLConfigFileStream != null)
                UAFLConfigFileStream.Close();
        }
    }
}
