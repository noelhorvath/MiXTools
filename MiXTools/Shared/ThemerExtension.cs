using Serilog;

namespace MiXTools.Shared
{
    public static class ThemerExtension
    {
        public static void SetTheme(this Button btn, Color foreColor, Color backColor, Color borderColor, int borderSize = 1, FlatStyle style = FlatStyle.Flat)
        {
            btn.ForeColor = foreColor;
            btn.BackColor = backColor;
            btn.FlatAppearance.BorderColor = borderColor;
            btn.FlatAppearance.BorderSize = borderSize;
            btn.FlatStyle = style;
        }

        public static void SetFont(this Button btn, float fontSize, FontFamily fontFamily, FontStyle fontStyle, GraphicsUnit graphicsUnit = GraphicsUnit.Pixel)
        {
            if (0 < fontSize)
            {
                try
                {
                    btn.Font = new Font(fontFamily, fontSize, fontStyle, graphicsUnit);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    Log.Error(ex.StackTrace);
                }
            }
        }

        public static void SetFont(this Button btn, Font font)
        {
            btn.Font = font;
        }

        public static void SetTheme(this Label label, Color foreColor, Color? backColor = null)
        {
            label.ForeColor = foreColor;
            if (backColor != null)
                label.BackColor = (Color)backColor;
        }
        public static void SetFont(this Label label, float fontSize, FontFamily fontFamily, FontStyle fontStyle, GraphicsUnit graphicsUnit = GraphicsUnit.Pixel)
        {
            if (0 < fontSize)
            {
                try
                {
                    label.Font = new Font(fontFamily, fontSize, fontStyle, graphicsUnit);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    Log.Error(ex.StackTrace);
                }
            }
        }

        public static void SetFont(this LinkLabel label, Font font)
        {
            label.Font = font;
        }

        public static void SetTheme(this LinkLabel label, Color linkColor, Color? backColor = null)
        {
            label.LinkColor = linkColor;
            Log.Debug("linklabel linkcolor: " + linkColor);
            if (backColor != null)
                label.BackColor = (Color)backColor;
        }
        public static void SetFont(this LinkLabel label, float fontSize, FontFamily fontFamily, FontStyle fontStyle, GraphicsUnit graphicsUnit = GraphicsUnit.Pixel)
        {
            if (0 < fontSize)
            {
                try
                {
                    label.Font = new Font(fontFamily, fontSize, fontStyle, graphicsUnit);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    Log.Error(ex.StackTrace);
                }
            }
        }

        public static void SetFont(this Label label, Font font)
        {
            label.Font = font;
        }

        public static void AddEventHandlers(this Button btn, EventHandler? mouseEnterHandler = null, EventHandler? mouseLeaveHandler = null, EventHandler? enabledChangedHandler = null)
        {
            if (mouseEnterHandler != null)
                btn.MouseEnter += mouseEnterHandler;

            if (mouseLeaveHandler != null)
                btn.MouseLeave += mouseLeaveHandler;

            if (enabledChangedHandler != null)
                btn.EnabledChanged += enabledChangedHandler;
        }
    }
}
