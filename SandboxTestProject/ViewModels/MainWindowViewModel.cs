using Microsoft.Extensions.Logging;
using System.Windows.Input;
using SandboxTestProject.ViewModels.Core;
using SandboxTestProject.ViewModels.Core.Interfaces;

namespace SandboxTestProject.ViewModels;

public class MainWindowViewModel : BaseViewModel, IMainWindowViewModel
{
    private readonly IHomeViewModel _homeViewModel;
    private readonly ISandBoxViewModel _sandBoxViewModel;
    private readonly ILogger<IMainWindowViewModel> _logger;

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

    public MainWindowViewModel(IHomeViewModel homeViewModel, ISandBoxViewModel sandBoxViewModel, ILogger<IMainWindowViewModel> logger)
    {
        _homeViewModel = homeViewModel;
        _sandBoxViewModel = sandBoxViewModel;
        _logger = logger;

        CurrentViewModel = _homeViewModel;

        _openSandboxCommand = new HandleCommand(obj => OpenSandbox());
        _closeSandboxCommand = new HandleCommand(obj => OpenHome());

        _homeViewModel.NavigateToSandboxCommand = _openSandboxCommand;
        _sandBoxViewModel.NavigateToHomeViewCommand = _closeSandboxCommand;
    }

    private void OpenHome()
    {
        try
        {
            _logger.LogInformation("Opening HomeView");

            CurrentViewModel = _homeViewModel;

            _logger.LogInformation("HomeView opened successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error opening HomeView");
        }
    }

    private void OpenSandbox()
    {
        try
        {
            _logger.LogInformation("Opening SandBoxView");

            CurrentViewModel = _sandBoxViewModel;

            _logger.LogInformation("SandBoxView opened successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error opening SandBoxView");
        }        
    }
}
