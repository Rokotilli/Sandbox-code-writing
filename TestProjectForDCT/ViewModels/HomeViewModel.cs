using System.Windows.Input;

namespace TestProjectForDCT.ViewModels;

public class HomeViewModel : BaseViewModel
{
    public ICommand NavigateToSandboxCommand { get; set; }

    public HomeViewModel()
    {
    }
}
