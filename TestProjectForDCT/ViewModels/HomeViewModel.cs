using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Windows;
using System.Windows.Input;
using TestProjectForDCT.ViewModels.Core;
using TestProjectForDCT.ViewModels.Core.Interfaces;

namespace TestProjectForDCT.ViewModels;

public class HomeViewModel : BaseViewModel, IHomeViewModel
{
    private readonly ISandBoxViewModel _sandBoxViewModel;
    private readonly ILogger<IHomeViewModel> _logger;
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

    public HomeViewModel(IOptions<Config> config, ISandBoxViewModel sandBoxViewModel, ILogger<IHomeViewModel> logger)
    {
        _config = config.Value;
        _sandBoxViewModel = sandBoxViewModel;
        _logger = logger;
        _localizationManager = LocalizationManager.GetInstance();

        _localizationManager.ChangeLanguage(_config.ApplicationLanguage);

        SwitchThemeButtonText = _config.ApplicationTheme;
        SwitchLanguageButtonText = _config.ApplicationLanguage;

        ApplicationSwitchThemeFile();

        SwitchThemeCommand = new HandleCommand(obj => SwitchTheme());
        SwitchLanguageCommand = new HandleCommand(obj => SwitchLanguage());
    }

    private void SwitchLanguage()
    {
        try
        {
            _logger.LogInformation("Switching language");

            var culture = _config.ApplicationLanguage == "en-US" ? "uk-UA" : "en-US";

            _localizationManager.ChangeLanguage(culture);

            _config.ApplicationLanguage = culture;

            _config.SaveConfig();

            SwitchLanguageButtonText = _config.ApplicationLanguage;

            SwitchThemeButtonText = _config.ApplicationTheme;

            _logger.LogInformation("Language switched successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while switching language");
        }
    }

    private void SwitchTheme()
    {
        try
        {
            _logger.LogInformation("Switching theme");

            _config.ApplicationTheme = _config.ApplicationTheme == "Light" ? "Dark" : "Light";

            _config.SaveConfig();

            ApplicationSwitchThemeFile();

            SwitchThemeButtonText = _config.ApplicationTheme;

            _sandBoxViewModel.UpdateSyntaxHighlighting();

            _logger.LogInformation("Theme switched successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while switching theme");
        }        
    }

    private void ApplicationSwitchThemeFile()
    {
        try
        {
            _logger.LogInformation("Switching theme file");

            var themeFile = _config.ApplicationTheme == "Light" ? "LightTheme.xaml" : "DarkTheme.xaml";

            var dict = new ResourceDictionary();

            dict.Source = new Uri($"Themes/{themeFile}", UriKind.Relative);

            var oldDict = Application.Current.Resources.MergedDictionaries.FirstOrDefault();

            if (oldDict != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(oldDict);
            }

            Application.Current.Resources.MergedDictionaries.Add(dict);

            _logger.LogInformation("Theme file switched successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while switching theme file");
        }
    }
}
