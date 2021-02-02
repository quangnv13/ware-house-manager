using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse_Manager.ViewModel
{
    public class NotificationViewModel
    {
        public string NotificationContent { get; set; }
        public string Title { get; set; }
        public NotificationViewModel(string content,string title)
        {
            NotificationContent = content;
            Title = title;
        }
    }
}
