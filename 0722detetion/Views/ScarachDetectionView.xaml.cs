using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _0722detetion.Views
{
    /// <summary>
    /// ScarachDetectionView.xaml 的交互逻辑
    /// </summary>
    public partial class ScarachDetectionView : UserControl
    {
        public ScarachDetectionView()
        {
            InitializeComponent();
            Loaded += ScarachDetectionView_OnLoaded;
        }

        private void ScarachDetectionView_OnLoaded(object sender, RoutedEventArgs e)
        {
          var vm = DataContext as ViewModel.ScarachDetectionViewModel;
            if (vm != null)
            {
                vm.hImage = halconimage.HalconWindow;
                vm.hResult = halconresult.HalconWindow;
            }
            else
            {
                MessageBox.Show("ViewModel is not set correctly.");
            }
        }
    }
}
