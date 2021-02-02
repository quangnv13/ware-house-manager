using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using WareHouse_Manager.UC;
using System.Windows.Controls;

namespace WareHouse_Manager.ViewModel
{
    public class ToolBarViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DelCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ToolBarViewModel()
        {
            AddCommand = new RelayCommand<UserControl>(x => { return true; }, 
                                x => 
                                {
                                    string title = (GetWindowParrent(x) as Window).Title;
                                    NotificationUC notification = new NotificationUC("Đã thêm", title);
                                    notification.ShowDialog();
                                });
            EditCommand = new RelayCommand<UserControl>(x => { return true; },
                                x =>
                                {
                                    string title = (GetWindowParrent(x) as Window).Title;
                                    NotificationUC notification = new NotificationUC("Đã sửa", title);
                                    notification.ShowDialog();
                                });
            DelCommand = new RelayCommand<UserControl>(x => { return true; },
                                x =>
                                {
                                    string title = (GetWindowParrent(x) as Window).Title;
                                    NotificationUC notification = new NotificationUC("Đã xóa", title);
                                    notification.ShowDialog();
                                });
            SaveCommand = new RelayCommand<UserControl>(x => { return true; },
                                x =>
                                {
                                    string title = (GetWindowParrent(x) as Window).Title;
                                    NotificationUC notification = new NotificationUC("Đã lưu thay đổi", title);
                                    notification.ShowDialog();
                                });
        }

        FrameworkElement GetWindowParrent(UserControl uc)
        {
            FrameworkElement e = uc.Parent as FrameworkElement;
            while (e.Parent != null)
            {
                e = e.Parent as FrameworkElement;
            }
            return e;
        }
    }
}
