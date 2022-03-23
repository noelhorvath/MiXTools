using System.ComponentModel;

namespace MiXTools.View
{
    public interface IViewLanguageManager
    {
        void UpdateTextLanguage();
        void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs args);
        void Unsubscribe();
    }
}
