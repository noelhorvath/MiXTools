using Serilog;
using System.ComponentModel;

namespace MiXTools.Shared
{
    public static class SettingsManager
    {
        public static void AddSettingsPropertyChangeEventHandler(PropertyChangedEventHandler handler)
        {
            Properties.Settings.Default.PropertyChanged += handler;
        }
        public static void RemoveSettingsPropertyChangeEventHandler(PropertyChangedEventHandler handler)
        {
            try
            {
                Properties.Settings.Default.PropertyChanged -= handler;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                Log.Error(e.StackTrace);
            }
        }
    }
}
