using System.Windows;
using System.Windows.Controls;

namespace SandboxTestProject.Views;

public partial class DetailsProblemView : UserControl
{
    public DetailsProblemView()
    {
        InitializeComponent();

        SizeChanged += DetailsProblemView_SizeChanged;
    }

    private void DetailsProblemView_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        double newWidth = e.NewSize.Width;

        double calculatedMaxWidth = Math.Min(newWidth * 0.9, double.MaxValue);

        ContentBorder.MaxWidth = calculatedMaxWidth;
    }
}
