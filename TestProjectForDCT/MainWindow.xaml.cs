using System.Windows;
using TestProjectForDCT.ViewModels;

namespace TestProjectForDCT;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel mainWindowViewModel)
    {
        InitializeComponent();

        DataContext = mainWindowViewModel;
    }
}