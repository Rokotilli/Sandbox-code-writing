using System.Windows.Input;

namespace SandboxTestProject.ViewModels.Core.Interfaces;

public interface IHomeViewModel
{
    string SwitchThemeButtonText { get; set; }
    string SwitchLanguageButtonText { get; set; }
    ICommand NavigateToSandboxCommand { get; set; }
    ICommand SwitchThemeCommand { get; set; }
    ICommand SwitchLanguageCommand { get; set; }
}
