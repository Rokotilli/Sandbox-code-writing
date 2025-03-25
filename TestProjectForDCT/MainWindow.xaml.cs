using System.Windows;
using TestProjectForDCT.ViewModels;
using TestProjectForDCT.ViewModels.Core.Interfaces;

namespace TestProjectForDCT;

public partial class MainWindow : Window
{
    public MainWindow(IMainWindowViewModel mainWindowViewModel)
    {
        InitializeComponent();

        DataContext = mainWindowViewModel;
    }
}