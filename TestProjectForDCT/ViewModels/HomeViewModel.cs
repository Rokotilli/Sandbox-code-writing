using System.Windows;
using System.Windows.Input;
using TestProjectForDCT.Extensions;
using TestProjectForDCT.ViewModels.CommandHandler;

namespace TestProjectForDCT.ViewModels;

public class HomeViewModel : BaseViewModel
{
    private SandBoxViewModel _sandBoxViewModel;
    private Config _config;
    private string _buttonText;

    public string ButtonText
    {
        get => _buttonText;
        set
        {
            _buttonText = value;
            OnPropertyChanged();
        }
    }

    public ICommand NavigateToSandboxCommand { get; set; }
    public ICommand SwitchThemeCommand { get; set; }

    public HomeViewModel(Config config, SandBoxViewModel sandBoxViewModel)
    {
        _config = config;
        _sandBoxViewModel = sandBoxViewModel;

        ButtonText = _config.ApplicationTheme == "Light" ? "Switch theme: Dark" : "Switch theme: Light";

        ApplicationSwitchThemeFile();

        SwitchThemeCommand = new HandleCommand(obj => SwitchTheme());
    }

    public void SwitchTheme()
    {   
        _config.ApplicationTheme = _config.ApplicationTheme == "Light" ? "Dark" : "Light";

        _config.SaveConfig();

        ApplicationSwitchThemeFile();

        ButtonText = _config.ApplicationTheme == "Light" ? "Switch theme: Dark" : "Switch theme: Light";

        _sandBoxViewModel.UpdateSyntaxHighlighting();
    }

    public void ApplicationSwitchThemeFile()
    {
        var themeFile = _config.ApplicationTheme == "Light" ? "LightTheme.xaml" : "DarkTheme.xaml";

        var dict = new ResourceDictionary();

        dict.Source = new Uri($"Themes/{themeFile}", UriKind.Relative);

        var oldDict = Application.Current.Resources.MergedDictionaries.FirstOrDefault();

        if (oldDict != null)
        {
            Application.Current.Resources.MergedDictionaries.Remove(oldDict);
        }

        Application.Current.Resources.MergedDictionaries.Add(dict);
    }
}
