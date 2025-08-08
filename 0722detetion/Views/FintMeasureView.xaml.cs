using System.Windows;
using System.Windows.Controls;

namespace _0722detetion.Views;

public partial class FintMeasureView : UserControl
{
    public FintMeasureView()
    {
        InitializeComponent();
        Loaded += FintMeasureView_OnLoaded;
    }

    private void FintMeasureView_OnLoaded(object sender, RoutedEventArgs e)
    {
        var vm = DataContext as ViewModel.FintMeasureViewModel;

        if (vm != null)
        {
           vm.hWindow = halconWindow.HalconWindow;
            vm.halcon = halconWindow;
            vm.MouseMove();
        }
        else
        {
            MessageBox.Show("ViewModel is not set correctly.");
        }
    }
}