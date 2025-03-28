using System.Windows;
using SandboxTestProject.ViewModels.Core.Interfaces;

namespace SandboxTestProject.Views;

public partial class LeetCodeProblemsView : Window
{
    public LeetCodeProblemsView(ILeetCodeProblemsViewModel leetCodeProblemsViewModel)
    {
        InitializeComponent();

        DataContext = leetCodeProblemsViewModel;

        leetCodeProblemsViewModel.InitializeData();
    }
}
