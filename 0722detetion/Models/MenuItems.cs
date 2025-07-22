using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0722detetion.Models
{
    public class MenuItems
    {
        public string Icon { get; set; }

        public string Name { get; set; }

        public string PageName { get; set; }
        public Action<object, object> PropertyChanged { get; internal set; }
    }
}
