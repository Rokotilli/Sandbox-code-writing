using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace TestProjectForDCT.ViewModels.Core;

public class LocalizationManager : INotifyPropertyChanged
{
    private readonly ResourceManager _resourceManager;

    private static LocalizationManager _instance = new();
    public static LocalizationManager GetInstance() => _instance;

    public event PropertyChangedEventHandler PropertyChanged;    

    private LocalizationManager() => _resourceManager = Resources.Strings.ResourceManager;

    public void ChangeLanguage(string culture)
    {
        if (string.IsNullOrEmpty(culture))
        {
            return;
        }

        var cultureInfo = new CultureInfo(culture);

        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    }

    public string this[string key] => _resourceManager.GetString(key, Thread.CurrentThread.CurrentUICulture);
}
