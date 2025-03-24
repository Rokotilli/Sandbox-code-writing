using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Windows.Input;
using TestProjectForDCT.Extensions;
using TestProjectForDCT.ViewModels.Core;

namespace TestProjectForDCT.ViewModels;

public class HomeViewModel : BaseViewModel
{
    private SandBoxViewModel _sandBoxViewModel;
    private LocalizationManager _localizationManager;
    private Config _config;
    private string _switchThemeButtonText;
    private string _switchLanguageButtonText;

    public string SwitchThemeButtonText
    {
        get => _switchThemeButtonText;
        set
        {

            var theme = value == "Light" ? _localizationManager["DarkThemeName"] : _localizationManager["LightThemeName"];
            _switchThemeButtonText = _localizationManager["SwitchTheme"] + theme;
            OnPropertyChanged();
        }
    }

    public string SwitchLanguageButtonText
    {
        get => _switchLanguageButtonText;
        set
        {
            var language = value == "en-US" ? _localizationManager["UkrainianLanguageName"] : _localizationManager["EnglishLanguageName"];
            _switchLanguageButtonText = _localizationManager["SwitchLanguage"] + language;
            OnPropertyChanged();
        }
    }

    public ICommand NavigateToSandboxCommand { get; set; }
    public ICommand SwitchThemeCommand { get; set; }
    public ICommand SwitchLanguageCommand { get; set; }

    public HomeViewModel(Config config, SandBoxViewModel sandBoxViewModel)
    {
        _config = config;
        _sandBoxViewModel = sandBoxViewModel;
        _localizationManager = LocalizationManager.GetInstance();

        _localizationManager.ChangeLanguage(config.ApplicationLanguage);

        SwitchThemeButtonText = _config.ApplicationTheme;
        SwitchLanguageButtonText = _config.ApplicationLanguage;

        ApplicationSwitchThemeFile();

        SwitchThemeCommand = new HandleCommand(obj => SwitchTheme());
        SwitchLanguageCommand = new HandleCommand(obj => SwitchLanguage());
    }

    private void SwitchLanguage()
    {
        var culture = _config.ApplicationLanguage == "en-US" ? "uk-UA" : "en-US";

        _localizationManager.ChangeLanguage(culture);

        _config.ApplicationLanguage = culture;

        _config.SaveConfig();

        SwitchLanguageButtonText = _config.ApplicationLanguage;

        SwitchThemeButtonText = _config.ApplicationTheme;
    }

    private void SwitchTheme()
    {   
        _config.ApplicationTheme = _config.ApplicationTheme == "Light" ? "Dark" : "Light";

        _config.SaveConfig();

        ApplicationSwitchThemeFile();

        SwitchThemeButtonText = _config.ApplicationTheme;

        _sandBoxViewModel.UpdateSyntaxHighlighting();
    }

    private void ApplicationSwitchThemeFile()
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
