namespace MiXTools.View
{
    internal interface IViewDarkMode : IViewSimpleDarkMode
    {
        FontFamily DefaultFontFamily { get; set; }
        Color FormBackgroundColor { get; set; }
        Color LabelFontColor { get; set; }
        Color LabelBackgroundColor { get; set; }
        FlatStyle BtnStyle { get; set; }
        Color BtnFontColor { get; set; }
        Color BtnBorderColor { get; set; }
        Color BtnBackgroundColor { get; set; }
        int BtnBorderSize { get; set; }
        Color BtnMouseEnterFontColor { get; set; }
        Color BtnMouseEnterBorderColor { get; set; }
        Color BtnMouseEnterBackgroundColor { get; set; }
        Color BtnDisabledFontColor { get; set; }
        Color BtnDisabledBorderColor { get; set; }
        Color BtnDisabledBackgroundColor { get; set; }
        GraphicsUnit FontUnit { get; set; }
        float NormalFontSize { get; set; }
        float BigFontSize { get; set; }
        float SmallFontSize { get; set; }
        float ExtraSmallFontSize { get; set; }
        FontStyle DefaultFontStyle { get; set; }
        const string FONT_SEGOE_UI = "Segoe UI";

        void ChangeButtonTheme_MouseEnter(object? sender, EventArgs args);
        void ChangeButtonTheme_MouseLeave(object? sender, EventArgs args);
        void ChangeButtonTheme_EnabledChanged(object? sender, EventArgs args);
        void InitializeButtonThemeChangingEventHandlers();
        void UpdateThemeProperties();
        void SetStaticThemeProperties();
        void InitTheme();
    }
}
