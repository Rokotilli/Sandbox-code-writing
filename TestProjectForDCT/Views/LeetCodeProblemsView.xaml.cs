using System.Windows;
using TestProjectForDCT.ViewModels;

namespace TestProjectForDCT.Views;

public partial class LeetCodeProblemsView : Window
{
    public LeetCodeProblemsView(LeetCodeProblemsViewModel leetCodeProblemsViewModel)
    {
        InitializeComponent();

        DataContext = leetCodeProblemsViewModel;

        leetCodeProblemsViewModel.InitializeData();
    }
}
