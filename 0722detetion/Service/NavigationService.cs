using _0722detetion.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace _0722detetion.Service
{
    public class NavigationService : BindableBase
    {
        public NavigationService()
        {

            Items = new ObservableCollection<MenuItems>();
            Inite();

        }



        private ObservableCollection<MenuItems> items;

        public ObservableCollection<MenuItems> Items
        {
            get { return items; }
            set { items = value; RaisePropertyChanged(); }
        }


        private void Inite()
        {
            items.Add(new MenuItems
            {
                Icon = "Monitor", // LCD相关图标
                Name = "LCD划痕检测",
                PageName = "ScarachDetectionView"
            });

            items.Add(new MenuItems
            {
                Icon = "ViewDashboard", // 仪表板图标
                Name = "拟合测量",
                PageName = "FintMeasureView"
            });

            items.Add(new MenuItems
            {
                Icon = "BottleWine", // 摄像头图标
                Name = "瓶口检测",
                PageName = "BottleDefectsView"
            });
            items.Add(new MenuItems
            {
                Icon = "HumanGreetingProximity", // Socket通信图标
                Name = "Socket通信",
                PageName = "SocketConnectionView"
            });

            // 可以添加更多菜单项
            items.Add(new MenuItems
            {
                Icon = "Cog", // 设置图标
                Name = "系统设置",
                PageName = "SettingsView"
            });

            items.Add(new MenuItems
            {
                Icon = "Information", // 信息图标
                Name = "关于",
                PageName = "AboutView"
            });
        }

    }
}
