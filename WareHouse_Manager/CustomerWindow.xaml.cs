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

namespace WareHouse_Manager
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    /// 
    public partial class CustomerWindow : Window
    {
        public CustomerViewModel ViewModel { get; set; }
        public CustomerWindow()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new CustomerViewModel();
        }
    }
}
