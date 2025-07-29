using _0722detetion.Models;
using _0722detetion.Service;

using System.Collections.ObjectModel;
using System.Windows;


namespace _0722detetion.ViewModel
{
    public class MainViewModel:BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly NavigationService _navigationService;
        public MainViewModel(IRegionManager regionManager,NavigationService navigationService)
        {
            _regionManager= regionManager;

            ItemDoubleClickCommand = new DelegateCommand<MenuItems>(ItemDoubleClick);
             _navigationService = navigationService;
            Items= _navigationService.Items;

        }

        

        private ObservableCollection<MenuItems> items;

        public ObservableCollection<MenuItems> Items
        {
            get { return items; }
            set { items = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<MenuItems> ItemDoubleClickCommand { get; set; }
        /// <summary>
        /// 导航到指定页面,之后回调触发页面的初始化方法
        /// </summary>
        /// <param name="items"></param>
        private void ItemDoubleClick(MenuItems item)
        {
            // 导航页面
            _regionManager.RequestNavigate("MainRegion", item.PageName, result =>
            {
                if (result.Success) 
                {
                    // 获取已加载视图（通过区域名）
                    var view = _regionManager.Regions["MainRegion"].ActiveViews.FirstOrDefault();
                    if (view is FrameworkElement element && element.DataContext is IResetStart resettable)
                    {
                        resettable.Reset();
                    }
                }
            });
        }



    }
}
