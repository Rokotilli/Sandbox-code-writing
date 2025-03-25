using System.Windows;
using TestProjectForDCT.ViewModels.Core.Interfaces;

namespace TestProjectForDCT.Views;

public partial class LeetCodeProblemsView : Window
{
    public LeetCodeProblemsView(ILeetCodeProblemsViewModel leetCodeProblemsViewModel)
    {
        InitializeComponent();

        DataContext = leetCodeProblemsViewModel;

        leetCodeProblemsViewModel.InitializeData();
    }
}
