using _0722detetion.ViewModel;
using _0722detetion.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace _0722detetion
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainView,MainViewModel>();
            containerRegistry.RegisterForNavigation<ScarachDetectionView,ScarachDetectionViewModel>();
        }
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
          
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
           
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }
    }
    
    

}
