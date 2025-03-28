using System.Windows;
using SandboxTestProject.ViewModels;
using SandboxTestProject.ViewModels.Core.Interfaces;

namespace SandboxTestProject;

public partial class MainWindow : Window
{
    public MainWindow(IMainWindowViewModel mainWindowViewModel)
    {
        InitializeComponent();

        DataContext = mainWindowViewModel;
    }
}