using System.Windows.Input;
using TestProjectForDCT.Extensions;
using TestProjectForDCT.ViewModels.CommandHandler;

namespace TestProjectForDCT.ViewModels;

public class EnteringPersonalDataViewModel : BaseViewModel
{
    private Config _config;
    private object _sessionTokenBorderBrushColor = "Gray";
    private object _csrfTokenBorderBrushColor = "Gray";
    private string _sessionToken;
    private string _csrfToken;

    public string SessionToken
    {
        get => _sessionToken;
        set
        {
            _sessionToken = value;
            OnPropertyChanged();
        }
    }

    public string CsrfToken
    {
        get => _csrfToken;
        set
        {
            _csrfToken = value;
            OnPropertyChanged();
        }
    }

    public object SessionTokenBorderBrushColor
    {
        get => _sessionTokenBorderBrushColor;
        set
        {
            _sessionTokenBorderBrushColor = value;
            OnPropertyChanged();
        }
    }

    public object CsrfTokenBorderBrushColor
    {
        get => _csrfTokenBorderBrushColor;
        set
        {
            _csrfTokenBorderBrushColor = value;
            OnPropertyChanged();
        }
    }

    public ICommand SaveDataCommand { get; }
    public ICommand NavigateLeetCodeProblemsView { get; set; }

    public EnteringPersonalDataViewModel(Config config)
    {
        _config = config;
        SaveDataCommand = new HandleCommand(obj => SaveData());
    }

    private void SaveData()
    {
        if (_sessionToken == null)
        {
            SessionTokenBorderBrushColor = "Red";
        }

        if (_csrfToken == null)
        {
            CsrfTokenBorderBrushColor = "Red";
        }

        if (_sessionToken == null || _csrfToken == null)
        {
            return;
        }

        _config.LeetCode.session_token = _sessionToken;
        _config.LeetCode.csrf_token = _csrfToken;

        _config.SaveConfig();

        NavigateLeetCodeProblemsView.Execute(null);
    }
}
