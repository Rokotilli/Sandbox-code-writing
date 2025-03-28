using System.Windows.Input;

namespace SandboxTestProject.ViewModels.Core.Interfaces;

public interface IEnteringPersonalDataViewModel
{
    string SessionToken { get; set; }
    string CsrfToken { get; set; }
    object SessionTokenBorderBrushColor { get; set; }
    object CsrfTokenBorderBrushColor { get; set; }
    ICommand SaveDataCommand { get; }
    ICommand NavigateLeetCodeProblemsView { get; set; }
}
