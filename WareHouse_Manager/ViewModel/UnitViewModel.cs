using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using WareHouse_Manager.Model;
using System.Windows.Input;
using System.Windows;
using WareHouse_Manager.UC;
using System.Windows.Controls;

namespace WareHouse_Manager.ViewModel
{
    public class UnitViewModel:BaseViewModel
    {
        private List<UNIT> _unitList;


        public List<UNIT> UnitList { get => _unitList; set { _unitList = value; OnPropertyChanged(); } }
        private ObservableCollection<Units> _units;
        public ObservableCollection<Units> Units { get=>_units; set {_units=value;OnPropertyChanged(); } }
        private Units _selectedItem;
        public Units SelectedItem { get => _selectedItem; set { _selectedItem = value;OnPropertyChanged();if (SelectedItem != null) DisplayName = _selectedItem.NAME; } }
        private string _displayName;
        public string DisplayName { get => _displayName; set { _displayName = value;OnPropertyChanged(); } }





        private int _cmd;
        public int Cmd { get => _cmd; set { _cmd = value;OnPropertyChanged(); } }
        private bool _isEnabled;
        public bool IsEnabled { get => _isEnabled; set { _isEnabled = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DelCommand { get; set; }
        public ICommand SaveCommand { get; set; }
       public bool EnableEdit { get => enableEdit; set { enableEdit = value; if (enableEdit == true) IsEnabled = false; else IsEnabled = true; OnPropertyChanged(); } }



        private bool enableEdit;

        public UnitViewModel()
        {
            LoadDefault();
            AddCommand = new RelayCommand<Window>(
                x =>
                {
                if (Cmd==1||Cmd==2)
                        return false;
                    return true;
                },
                x =>
                {
                    DisplayName = "";
                    EnableEdit = true;
                    Cmd = 1;
                });
            EditCommand = new RelayCommand<Window>(
                x =>
                {
                    if (String.IsNullOrEmpty(DisplayName)||SelectedItem==null||Cmd==2 ||Cmd==1)
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
                if (SelectedItem == null||Cmd==1||Cmd==2)
                        return false;
                    return true;
                },
                x =>
                {
                    MessageBoxResult boxResult= MessageBox.Show("Bạn có muốn xóa không!!", "Xóa mục này",MessageBoxButton.YesNo,MessageBoxImage.Warning);
                    if (boxResult == MessageBoxResult.Yes)
                    {
                        var unit = DataProvider.Instance.DB.UNIT.Where(y => y.ID == SelectedItem.ID).SingleOrDefault();
                        DataProvider.Instance.DB.UNIT.Remove(unit);
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
                    UNIT unit = new UNIT() { NAME = DisplayName };
                    if (Cmd == 1)
                    {
                        var unitList = DataProvider.Instance.DB.UNIT.Where(y => y.NAME == unit.NAME);
                        if (unitList != null && unitList.Count() != 0)
                        {
                            notification("Đã tồn tại tên này", x.Title);
                        }
                        else if (String.IsNullOrEmpty(DisplayName))
                        {
                            notification("Vui lòng điền đầy đủ các trường yêu cầu", x.Title);
                        }
                        else
                        {
                            DataProvider.Instance.DB.UNIT.Add(unit);
                            DataProvider.Instance.DB.SaveChanges();
                            notification("Đã thêm thành công", x.Title);
                        }
                    }
                    if(Cmd==2)
                    {
                        var displayList = DataProvider.Instance.DB.UNIT.Where(y => y.NAME == DisplayName);
                        if (displayList != null && displayList.Count() != 0)
                        {
                            notification("Trùng tên đã có", x.Title);
                        }
                        else if(String.IsNullOrEmpty(DisplayName))
                        {
                            notification("Vui lòng điền đầy đủ các trường yêu cầu",x.Title);
                        }
                        else
                        {
                            var item = DataProvider.Instance.DB.UNIT.Where(y => y.ID == SelectedItem.ID).SingleOrDefault();
                            item.NAME = DisplayName;
                            DataProvider.Instance.DB.SaveChanges();
                            notification("Đã sửa thành công", x.Title);
                        }
                    }
                    LoadDefault();
                });
        }
        void LoadDefault()
        {
            UnitList = new List<UNIT>(DataProvider.Instance.DB.UNIT);
            Units = new ObservableCollection<Units>();
            int i = 1;
            foreach(var item in UnitList)
            {
                Units units = new Units();
                units.ID = item.ID;
                units.NAME = item.NAME;
                units.STT = i;
                Units.Add(units);
                i++;
            }
            EnableEdit = false;
            Cmd = 0;
        }
        void notification(string notification,string title)
        {
            NotificationUC notificationWindow= new NotificationUC(notification,"Thông báo -- "+ title);
            notificationWindow.ShowDialog();
        }
    }
}
