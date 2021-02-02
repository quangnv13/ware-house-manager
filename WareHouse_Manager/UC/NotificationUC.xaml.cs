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
using System.Windows.Shapes;
using WareHouse_Manager.ViewModel;

namespace WareHouse_Manager.UC
{
    /// <summary>
    /// Interaction logic for NotificationUC.xaml
    /// </summary>
    public partial class NotificationUC : Window
    {
        public NotificationViewModel ViewModel { get; set; }
        public NotificationUC(string content,string title)
        {
            InitializeComponent();
            ViewModel = new NotificationViewModel(content,title);
            this.DataContext = ViewModel;
        }
    }
}
