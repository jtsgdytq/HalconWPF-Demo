using _0722detetion.Models;
using _0722detetion.ViewModel;
using _0722detetion.Views;
using System.Configuration;
using System.Data;
using System.Windows;
using BottleDefectsView = _0722detetion.Views.BottleDefectsView;

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
            containerRegistry.RegisterForNavigation<FintMeasureView,FintMeasureViewModel>();
            containerRegistry.RegisterForNavigation<BottleDefectsView,BottleDefectsViewModel>();
            containerRegistry.RegisterSingleton<BottlesParamModel>();
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
