using MiXTools.Shared;
using System.ComponentModel;


namespace MiXTools.View
{
    public partial class CustomMessageBox : Form, IViewDarkMode, IViewLanguageManager
    {
        private readonly LanguageManager languageManager;
        public FontFamily DefaultFontFamily { get; set; }
        public Color FormBackgroundColor { get; set; }
        public Color LabelFontColor { get; set; }
        public Color LabelBackgroundColor { get; set; }
        public FlatStyle BtnStyle { get; set; }
        public Color BtnFontColor { get; set; }
        public Color BtnBorderColor { get; set; }
        public Color BtnBackgroundColor { get; set; }
        public int BtnBorderSize { get; set; }
        public Color BtnMouseEnterFontColor { get; set; }
        public Color BtnMouseEnterBorderColor { get; set; }
        public Color BtnMouseEnterBackgroundColor { get; set; }
        public Color BtnDisabledFontColor { get; set; }
        public Color BtnDisabledBorderColor { get; set; }
        public Color BtnDisabledBackgroundColor { get; set; }
        public GraphicsUnit FontUnit { get; set; }
        public float NormalFontSize { get; set; }
        public float BigFontSize { get; set; }
        public float SmallFontSize { get; set; }
        public float ExtraSmallFontSize { get; set; }
        public FontStyle DefaultFontStyle { get; set; }
        public bool DarkMode { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public CustomMessageBox(string title, string message) : this(title, message, LanguageManager.Instance) { }
        public CustomMessageBox(string title, string message, LanguageManager languageManager)
        {
            this.languageManager = languageManager;
            this.Title = this.languageManager.GetTranslation(title);
            this.Message = this.languageManager.GetTranslation(message);
            // disable resize
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;

            InitializeComponent();
            InitTheme();
        }
        public void ChangeButtonTheme_EnabledChanged(object? sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        public void ChangeButtonTheme_MouseEnter(object? sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        public void ChangeButtonTheme_MouseLeave(object? sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        public void ChangeDarkMode()
        {
            throw new NotImplementedException();
        }

        public void InitializeButtonThemeChangingEventHandlers()
        {
            throw new NotImplementedException();
        }

        public void InitTheme()
        {
            throw new NotImplementedException();
        }

        public void SetStaticThemeProperties()
        {
            throw new NotImplementedException();
        }

        public void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe()
        {
            throw new NotImplementedException();
        }

        public void UpdateTextLanguage()
        {
            throw new NotImplementedException();
        }

        public void UpdateThemeProperties()
        {
            throw new NotImplementedException();
        }
    }
}
