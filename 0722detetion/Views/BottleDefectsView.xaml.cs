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
    /// BottleDefectsView.xaml 的交互逻辑
    /// </summary>
    public partial class BottleDefectsView : UserControl
    {
        public BottleDefectsView()
        {
            InitializeComponent();
            Loaded += BottleDefectsView_Loaded;
        }

        private void BottleDefectsView_Loaded(object sender, RoutedEventArgs e)
        {
           var vm = DataContext as ViewModel.BottleDefectsViewModel;
            if (vm!= null)
            {
              vm.hsmart_1 = hsmart_1;
                vm.hsmart_2 = hsmart_2;
            }
        }
    }
}
