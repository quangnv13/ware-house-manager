using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WareHouse_Manager.Model;
using WareHouse_Manager.UC;

namespace WareHouse_Manager.ViewModel
{
    public class CustomerViewModel:BaseViewModel
    {
        private List<CUSTOMER> _listCustomer;
        public List<CUSTOMER> ListCustomer { get=>_listCustomer; set {_listCustomer=value;OnPropertyChanged(); } }
        private ObservableCollection<Customers> _customers;
        public ObservableCollection<Customers> Customers { get=>_customers; set {_customers=value;OnPropertyChanged(); } }
        private int _cmd;
        public int Cmd
        {
            get =>_cmd;
            set
            {
                _cmd =value;
                if(Cmd==1||Cmd==2)
                {
                    EnableEdit = true;
                    IsEnabled = false;
                }
                else
                {
                    EnableEdit = false;
                    IsEnabled = true;
                }
                OnPropertyChanged();
            }
        }

        private string _displayName;
        public string DisplayName { get => _displayName; set { _displayName = value; OnPropertyChanged(); } }
        private string _address;
        public string Address { get => _address; set { _address = value; OnPropertyChanged(); } }
        private string _phone;
        public string Phone { get => _phone; set { _phone = value; OnPropertyChanged(); } }
        private string _email;
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        private string _moreInfo;
        public string MoreInfo { get => _moreInfo; set { _moreInfo = value; OnPropertyChanged(); } }
        private int _regular;
        public int Regular { get=>_regular; set {_regular=value;OnPropertyChanged(); } }
        private bool _enableEdit;
        public bool EnableEdit { get=>_enableEdit; set {_enableEdit=value;OnPropertyChanged(); } }
        private bool _isEnabled;
        public bool IsEnabled { get=>_isEnabled ; set {_isEnabled=value;OnPropertyChanged(); } }

        private Customers _selectedItem;
        public Customers SelectedItem
        {
            get =>_selectedItem;
            set
            {
                _selectedItem =value;
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.NAME;
                    Address = SelectedItem.ADDRESS;
                    Phone = SelectedItem.PHONE;
                    Email = SelectedItem.EMAIL;
                    MoreInfo = SelectedItem.MORE_INFO;
                    Regular = SelectedItem.REGULAR;
                }
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DelCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public CustomerViewModel()
        {
            LoadDefault();

            AddCommand = new RelayCommand<Window>(
                x =>
                {
                    if (Cmd == 1 || Cmd == 2)
                        return false;
                    return true;
                },
                x =>
                {
                    DisplayName = "";
                    Address = "";
                    Phone = "";
                    Email = "";
                    MoreInfo = "";
                    Regular = 1;
                    Cmd = 1;
                });
            EditCommand = new RelayCommand<Window>(
                x =>
                {
                    if (String.IsNullOrEmpty(DisplayName) || SelectedItem == null || Cmd == 2 || Cmd == 1)
                        return false;
                    return true;
                },
                x =>
                {
                    Cmd = 2;
                });
            DelCommand = new RelayCommand<Window>(
                x =>
                {
                    if (SelectedItem == null || Cmd == 1 || Cmd == 2)
                        return false;
                    return true;
                },
                x =>
                {
                    MessageBoxResult boxResult = MessageBox.Show("Bạn có muốn xóa không!!", "Xóa mục này", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (boxResult == MessageBoxResult.Yes)
                    {
                        var customer = DataProvider.Instance.DB.CUSTOMER.Where(y => y.ID == SelectedItem.ID).SingleOrDefault();
                        DataProvider.Instance.DB.CUSTOMER.Remove(customer);
                        DataProvider.Instance.DB.SaveChanges();
                        notification("Đã xóa!!", x.Title);
                    }
                    else
                    {
                        notification("Đã hủy thao tác", x.Title);
                    }
                    LoadDefault();
                });
            SaveCommand = new RelayCommand<Window>(
                x =>
                {
                    if (Cmd != 0)
                        return true;
                    return false;
                },
                x =>
                {
                    CUSTOMER customer = new CUSTOMER() { NAME = DisplayName, ADDRESS = Address, PHONE = Phone, EMAIL = Email, MORE_INFO = MoreInfo, REGULAR= Regular };
                    if (Cmd == 1)
                    {
                        if (String.IsNullOrEmpty(DisplayName) || String.IsNullOrEmpty(Address) || String.IsNullOrEmpty(Phone) || String.IsNullOrEmpty(Email) || String.IsNullOrEmpty(MoreInfo) || String.IsNullOrEmpty(Regular.ToString()))
                        {
                            notification("Vui lòng điền đầy đủ các trường yêu cầu", x.Title);
                        }
                        else
                        {
                            DataProvider.Instance.DB.CUSTOMER.Add(customer);
                            DataProvider.Instance.DB.SaveChanges();
                            notification("Đã thêm thành công", x.Title);
                        }
                    }
                    if (Cmd == 2)
                    {
                        if (String.IsNullOrEmpty(DisplayName) || String.IsNullOrEmpty(Address) || String.IsNullOrEmpty(Phone) || String.IsNullOrEmpty(Email) || String.IsNullOrEmpty(MoreInfo) || String.IsNullOrEmpty(Regular.ToString()))
                        {
                            notification("Vui lòng điền đầy đủ các trường yêu cầu", x.Title);
                        }
                        else
                        {
                            var item = DataProvider.Instance.DB.CUSTOMER.Where(y => y.ID == SelectedItem.ID).SingleOrDefault();
                            item.NAME = DisplayName;
                            item.ADDRESS = Address;
                            item.PHONE = Phone;
                            item.EMAIL = Email;
                            item.MORE_INFO = MoreInfo;
                            item.REGULAR = Regular;

                            DataProvider.Instance.DB.SaveChanges();

                            notification("Đã sửa thành công", x.Title);
                        }
                    }
                    LoadDefault();
                });
        }
        void LoadDefault()
        {
            Customers = new ObservableCollection<Customers>();
            var customers = DataProvider.Instance.DB.CUSTOMER;
            int i = 1;
            foreach(var item in customers)
            {
                Customers customer = new Customers();
                customer.NAME = item.NAME;
                customer.ID = item.ID;
                customer.PHONE = item.PHONE;
                customer.MORE_INFO = item.MORE_INFO;
                customer.ADDRESS = item.ADDRESS;
                customer.EMAIL = item.EMAIL;
                customer.REGULAR = (int)item.REGULAR;
                customer.STT = i;
                i++;
                Customers.Add(customer);
            }
            Cmd = 0;
            SelectedItem = null;
        }
        void notification(string notification, string title)
        {
            NotificationUC notificationWindow = new NotificationUC(notification, "Thông báo -- " + title);
            notificationWindow.ShowDialog();
        }
    }
}
