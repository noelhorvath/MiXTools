
using System.ComponentModel;

namespace MiXTools.View
{
    internal interface IViewSimpleDarkMode
    {
        bool DarkMode { get; set; }
        void ChangeDarkMode();
        void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs args);
        void Unsubscribe();
    }
}
