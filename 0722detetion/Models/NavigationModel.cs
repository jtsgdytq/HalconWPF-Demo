using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace _0722detetion.Models
{
    public class NavigationModel : INavigationAware
    {
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
           return true; 
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
           
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }
    }
}
