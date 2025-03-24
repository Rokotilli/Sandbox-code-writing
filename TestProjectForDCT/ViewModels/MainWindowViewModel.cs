using System.Windows.Input;
using TestProjectForDCT.ViewModels.Core;

namespace TestProjectForDCT.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private readonly HomeViewModel _homeViewModel;
    private readonly SandBoxViewModel _sandBoxViewModel;
    private ICommand _openSandboxCommand;
    private ICommand _closeSandboxCommand;
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
        _closeSandboxCommand = new HandleCommand(obj => CurrentViewModel = _homeViewModel);

        _homeViewModel.NavigateToSandboxCommand = _openSandboxCommand;
        _sandBoxViewModel.NavigateToHomeViewCommand = _closeSandboxCommand;
    }

    private void OpenSandbox()
    {
        CurrentViewModel = _sandBoxViewModel;
    }
}
