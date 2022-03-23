using MiXTools.Database;
using MiXTools.Shared;
using Serilog;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MiXTools.View
{
    public partial class SettingsForm : Form, INotifyPropertyChanged, IViewDarkMode, IViewLanguageManager
    {
        private readonly LanguageManager languageManager;
        private readonly DatabaseManager databaseManager;
        private bool _isDataReset;
        public event PropertyChangedEventHandler? PropertyChanged;
        public bool IsDataReset
        {
            get { return _isDataReset; }
            set
            {
                if (_isDataReset != value)
                {
                    _isDataReset = value;
                    OnPropertyChanged();
                }
            }
        }
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
        public SettingsForm() : this(LanguageManager.Instance, DatabaseManager.Instance) { }

        public SettingsForm(LanguageManager languageManager, DatabaseManager databaseManager)
        {
            // disable resize
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            // set form position
            StartPosition = FormStartPosition.WindowsDefaultLocation;

            DefaultFontFamily = FontFamily.GenericSansSerif;
            this.languageManager = languageManager;
            this.databaseManager = databaseManager;
            InitializeComponent();
            InitTheme();
            Log.Debug(languageManager.GetIndexOfCurrentLanugage().ToString());
            Log.Debug(languageManager.CurrentLanguage);
            languagesComboBox.DataSource = languageManager.Languages;
            SettingsManager.AddSettingsPropertyChangeEventHandler(Settings_PropertyChanged);
            languagesComboBox.SelectedIndexChanged += new EventHandler(LanguagesComboBox_SelectedIndexChanged);
            languagesComboBox.SelectedIndex = languageManager.GetIndexOfCurrentLanugage();
            Log.Debug(languageManager.CurrentLanguage);
            Log.Debug(languageManager.GetIndexOfCurrentLanugage().ToString());
            UpdateTextLanguage();
            InitCheckBoxes();
            IsDataReset = false;
        }
        public void InitTheme()
        {
            DarkMode = Properties.Settings.Default.DarkMode;
            SetStaticThemeProperties();
            ChangeDarkMode();
            InitializeButtonThemeChangingEventHandlers();
        }

        public void IsPropertyChanged() { Log.Debug("is PropertyChanged: " + PropertyChanged != null ? "true" : "false"); }

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
            BtnBorderSize = 1;
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
            SmallFontSize = 25;
            NormalFontSize = 30;
            BigFontSize = 35;
        }

        private void InitCheckBoxes()
        {
            enableDarkModeCheckBox.Checked = DarkMode;
            logConsoleCheckBox.Checked = Properties.Settings.Default.OpenLogConsoleOnStartup;
        }

        public void UpdateTextLanguage()
        {
            try
            {
                Log.Debug($"Changing SettingsForm display language to {languageManager.CurrentLanguage}...");
                logConsoleOnStartupLabel.Text = languageManager.GetTranslation("LOG_CONSOLE_ON_STARTUP");
                darkModeLabel.Text = languageManager.GetTranslation("DARK_MODE");
                devSettingsLabel.Text = languageManager.GetTranslation("DEV_SETTINGS");
                languageLabel.Text = languageManager.GetTranslation("LANGUAGE");
                openLogConsoleButton.Text = languageManager.GetTranslation("OPEN_LOG_CONSOLE");
                resetAppDataButton.Text = languageManager.GetTranslation("RESET_APPDATA");
                applicationSettingsLabel.Text = languageManager.GetTranslation("APPLICATION_SETTINGS");
                this.Text = languageManager.GetTranslation("SETTINGS");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

        }

        private void LanguagesComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (sender != null)
            {
                ComboBox comboBox = (ComboBox)sender;
                Log.Debug("Selected language: " + comboBox.SelectedValue.ToString());
                if (comboBox.SelectedValue as string != languageManager.CurrentLanguage)
                {
                    if (comboBox.SelectedValue != null)
                    {
                        languageManager.ChangeLanguage((string)comboBox.SelectedValue);
                    }
                }
            }
        }

        private void EnableDarkModeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (sender != null)
            {
                CheckBox checkBox = (CheckBox)sender;
                if (Properties.Settings.Default.DarkMode != checkBox.Checked)
                {
                    Properties.Settings.Default.DarkMode = checkBox.Checked;
                    Properties.Settings.Default.Save();
                }
            }

        }
        private void LogConsoleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (sender != null)
            {
                CheckBox checkBox = (CheckBox)sender;
                if (Properties.Settings.Default.OpenLogConsoleOnStartup != checkBox.Checked)
                {
                    Properties.Settings.Default.OpenLogConsoleOnStartup = checkBox.Checked;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void OpenLogConsoleButton_Click(object sender, EventArgs e)
        {
            if (!Utils.IsLogFormOpen)
            {
                Utils.OpenLogConsole();
            }
        }

        private void ResetAppDataButton_Click(object sender, EventArgs e)
        {
            try
            {
                Log.Information("Resetting user AppData...");
                string[] appDataFiles = Directory.GetFiles(Application.UserAppDataPath);
                foreach (string file in appDataFiles)
                {
                    if (!file.Contains(databaseManager.DBFileName))
                        IOHelper.DeleteFile(file);
                    else
                        databaseManager.ResetDatabase();
                }
                IsDataReset = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
            }

        }

        public void ChangeDarkMode()
        {
            UpdateThemeProperties();
            Utils.ChangeWindowDarkMode(this.Handle, DarkMode);
            this.BackColor = FormBackgroundColor;
            // labels theme
            devSettingsLabel.SetTheme(LabelFontColor, LabelBackgroundColor);
            applicationSettingsLabel.SetTheme(LabelFontColor, LabelBackgroundColor);
            languageLabel.SetTheme(LabelFontColor, LabelBackgroundColor);
            darkModeLabel.SetTheme(LabelFontColor, LabelBackgroundColor);
            logConsoleOnStartupLabel.SetTheme(LabelFontColor, LabelBackgroundColor);
            // labels font
            devSettingsLabel.SetFont(BigFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            applicationSettingsLabel.SetFont(BigFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            languageLabel.SetFont(NormalFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            darkModeLabel.SetFont(NormalFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            logConsoleOnStartupLabel.SetFont(NormalFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            // button theme
            openLogConsoleButton.SetTheme(BtnFontColor, BtnBackgroundColor, BtnBorderColor, BtnBorderSize, BtnStyle);
            resetAppDataButton.SetTheme(BtnFontColor, BtnBackgroundColor, BtnBorderColor, BtnBorderSize, BtnStyle);
            // button font
            openLogConsoleButton.SetFont(NormalFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);
            resetAppDataButton.SetFont(NormalFontSize, DefaultFontFamily, DefaultFontStyle, FontUnit);

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

        public void InitializeButtonThemeChangingEventHandlers()
        {
            openLogConsoleButton.AddEventHandlers(
                ChangeButtonTheme_MouseEnter,
                ChangeButtonTheme_MouseLeave,
                ChangeButtonTheme_EnabledChanged
            );
            resetAppDataButton.AddEventHandlers(
                ChangeButtonTheme_MouseEnter,
                ChangeButtonTheme_MouseLeave,
                ChangeButtonTheme_EnabledChanged
            );
        }

        public void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs args)
        {
            Log.Debug($"Settings property {args.PropertyName} changed in SettingsForm");
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
            Log.Debug("Unsubscribing from events...");
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
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
