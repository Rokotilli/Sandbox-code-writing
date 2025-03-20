using System.Windows.Input;
using TestProjectForDCT.ViewModels.CommandHandler;

namespace TestProjectForDCT.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private readonly HomeViewModel _homeViewModel;
    private readonly SandBoxViewModel _sandBoxViewModel;
    private ICommand _openSandboxCommand;
    private object _currentViewModel;

    public object CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged();
        }
    }    

    public MainWindowViewModel(HomeViewModel homeViewModel, SandBoxViewModel sandBoxViewModel)
    {
        _homeViewModel = homeViewModel;
        _sandBoxViewModel = sandBoxViewModel;

        CurrentViewModel = _homeViewModel;

        _openSandboxCommand = new HandleCommand(obj => OpenSandbox());

        _homeViewModel.NavigateToSandboxCommand = _openSandboxCommand;
    }

    private void OpenSandbox()
    {
        CurrentViewModel = _sandBoxViewModel;
    }
}
