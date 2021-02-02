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
    public class SuplierViewModel:BaseViewModel
    {
        private List<SUPLIER> _suplierList;
        public List<SUPLIER> SuplierList { get => _suplierList; set { _suplierList = value; OnPropertyChanged(); } }
        private ObservableCollection<Supliers> _supliers;
        public ObservableCollection<Supliers> Supliers { get=>_supliers; set {_supliers=value;OnPropertyChanged(); } }
        private Supliers _selectedItem;
        public Supliers SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = _selectedItem.NAME;
                    Address = _selectedItem.ADDRESS;
                    Phone = _selectedItem.PHONE;
                    Email = _selectedItem.EMAIL;
                    MoreInfo = _selectedItem.MORE_INFO;
                    ConstractDate = (DateTime)_selectedItem.CONSTRACT_DATE;
                }
            }
        }
        private string _displayName;
        public string DisplayName { get => _displayName; set { _displayName = value; OnPropertyChanged(); } }
        private string _address;
        public string Address { get => _address; set { _address = value;OnPropertyChanged(); } }
        private string _phone;
        public string Phone { get=>_phone; set { _phone = value;OnPropertyChanged() ; } }
        private string _email;
        public string Email { get => _email; set { _email = value;OnPropertyChanged(); } }
        private string _moreInfo;
        public string MoreInfo { get => _moreInfo; set { _moreInfo = value; OnPropertyChanged(); } }
        private DateTime _constractDate;
        public DateTime ConstractDate { get => _constractDate; set { _constractDate = value; OnPropertyChanged(); } }
        private int _cmd;
        public int Cmd { get => _cmd; set { _cmd = value; OnPropertyChanged(); } }
        private bool _isEnabled;
        public bool IsEnabled { get => _isEnabled; set { _isEnabled = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DelCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public bool EnableEdit { get => enableEdit; set { enableEdit = value; if (enableEdit == true) IsEnabled = false; else IsEnabled = true; OnPropertyChanged(); } }



        private bool enableEdit;

        public SuplierViewModel()
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
                    ConstractDate = DateTime.Now;
                    EnableEdit = true;
                    Cmd = 1;
                });
            EditCommand = new RelayCommand<Window>(
                x =>
                {
                    if (String.IsNullOrEmpty(DisplayName) || SelectedItem == null || Cmd == 2 || Cmd==1)
                        return false;
                    return true;
                },
                x =>
                {
                    Cmd = 2;
                    EnableEdit = true;
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
                        var suplier = DataProvider.Instance.DB.SUPLIER.Where(y => y.ID == SelectedItem.ID).SingleOrDefault();
                        DataProvider.Instance.DB.SUPLIER.Remove(suplier);
                        DataProvider.Instance.DB.SaveChanges();
                        notification("Đã xóa!!",x.Title);
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
                    SUPLIER suplier = new SUPLIER() { NAME = DisplayName,ADDRESS=Address,PHONE=Phone,EMAIL=Email,MORE_INFO=MoreInfo,CONSTRACT_DATE=ConstractDate };
                    if (Cmd == 1)
                    {
                        var suplierList = DataProvider.Instance.DB.SUPLIER.Where(y => y.NAME == suplier.NAME);
                        if (suplierList != null && suplierList.Count() != 0)
                        {
                            notification("Đã tồn tại tên này", x.Title);
                        }
                        else if (String.IsNullOrEmpty(DisplayName) || String.IsNullOrEmpty(Address) || String.IsNullOrEmpty(Phone) || String.IsNullOrEmpty(Email) || String.IsNullOrEmpty(MoreInfo) || String.IsNullOrEmpty(ConstractDate.ToString()))
                        {
                            notification("Vui lòng điền đầy đủ các trường yêu cầu", x.Title);
                        }
                        else
                        {
                            try
                            {
                                DataProvider.Instance.DB.SUPLIER.Add(suplier);
                                DataProvider.Instance.DB.SaveChanges();
                                notification("Đã thêm thành công", x.Title);
                            }
                            catch
                            {
                                notification("Có lỗi xảy ra!! Kiểm tra lại các trường thời gian", x.Title);
                            }
                        }
                    }
                    if (Cmd == 2)
                    {
                        //var displayList = DataProvider.Instance.DB.SUPLIERs.Where(y => y.NAME == DisplayName);
                        //if (displayList != null && displayList.Count() != 0)
                        //{
                        //    notification("Trùng tên đã có", x.Title);
                        //    LoadDefault();
                        //}
                        //else 
                        if (String.IsNullOrEmpty(DisplayName) || String.IsNullOrEmpty(Address) || String.IsNullOrEmpty(Phone) || String.IsNullOrEmpty(Email) || String.IsNullOrEmpty(MoreInfo) || String.IsNullOrEmpty(ConstractDate.ToString()))
                        {
                            notification("Vui lòng điền đầy đủ các trường yêu cầu", x.Title);
                        }
                        else
                        {
                            var item = DataProvider.Instance.DB.SUPLIER.Where(y => y.ID == SelectedItem.ID).SingleOrDefault();
                            item.NAME = DisplayName;
                            item.ADDRESS = Address;
                            item.PHONE = Phone;
                            item.EMAIL = Email;
                            item.MORE_INFO = MoreInfo;
                            item.CONSTRACT_DATE = ConstractDate;

                            DataProvider.Instance.DB.SaveChanges();

                            notification("Đã sửa thành công", x.Title);
                        }
                    }
                    LoadDefault();
                });
        }
        void LoadDefault()
        {
            SuplierList = new List<SUPLIER>(DataProvider.Instance.DB.SUPLIER);
            Supliers = new ObservableCollection<Supliers>();
            int i = 1;
            foreach(var item in SuplierList)
            {
                Supliers suplier = new Supliers();
                suplier.ID = item.ID;
                suplier.NAME = item.NAME;
                suplier.PHONE = item.PHONE;
                suplier.EMAIL = item.EMAIL;
                suplier.ADDRESS = item.ADDRESS;
                suplier.MORE_INFO = item.MORE_INFO;
                suplier.STT = i;
                suplier.CONSTRACT_DATE = (DateTime)item.CONSTRACT_DATE;
                Supliers.Add(suplier);
                i++;
            }
            EnableEdit = false;
            Cmd = 0;
        }
        void notification(string notification, string title)
        {
            NotificationUC notificationWindow = new NotificationUC(notification, "Thông báo -- " + title);
            notificationWindow.ShowDialog();
        }
    }
}
