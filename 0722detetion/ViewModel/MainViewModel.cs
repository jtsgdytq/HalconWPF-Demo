using _0722detetion.Models;
using _0722detetion.Service;

using System.Collections.ObjectModel;


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

        private void ItemDoubleClick(MenuItems items)
        {
            _regionManager.RequestNavigate("MainRegion", items.PageName);
        }
    }
}
