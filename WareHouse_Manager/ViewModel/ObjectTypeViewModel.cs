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
    public class ObjectTypeViewModel:BaseViewModel
    {
        private ObservableCollection<ObjectType> _objectTypes;
        public ObservableCollection<ObjectType>  ObjectTypes{ get=>_objectTypes; set {_objectTypes=value;OnPropertyChanged(); } }
        private List<OBJECT_TYPE> _objectTypeList;
        public List<OBJECT_TYPE> ObjectTypeList { get => _objectTypeList; set { _objectTypeList = value; OnPropertyChanged(); } }
        private ObjectType _selectedItem;
        public ObjectType SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = _selectedItem.NAME;
                }
            }
        }
        private string _displayName;
        public string DisplayName { get=>_displayName; set {_displayName=value; OnPropertyChanged(); } }

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

        public ObjectTypeViewModel()
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
                    EnableEdit = true;
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
                        var object_type = DataProvider.Instance.DB.OBJECT_TYPE.Where(y => y.ID == SelectedItem.ID).SingleOrDefault();
                        DataProvider.Instance.DB.OBJECT_TYPE.Remove(object_type);
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
                    OBJECT_TYPE object_type = new OBJECT_TYPE() { NAME = DisplayName};
                    if (Cmd == 1)
                    {
                        var objectTypeList = DataProvider.Instance.DB.OBJECT_TYPE.Where(y => y.NAME == object_type.NAME);
                        if (objectTypeList != null && objectTypeList.Count() != 0)
                        {
                            notification("Đã tồn tại tên này", x.Title);
                        }
                        else if (String.IsNullOrEmpty(DisplayName))
                        {
                            notification("Vui lòng điền đầy đủ các trường yêu cầu", x.Title);
                        }
                        else
                        {
                            try
                            {
                                DataProvider.Instance.DB.OBJECT_TYPE.Add(object_type);
                                DataProvider.Instance.DB.SaveChanges();
                                notification("Đã thêm thành công", x.Title);
                            }
                            catch
                            {
                                notification("Có lỗi xảy ra!! Kiểm tra lại các trường nhập vào", x.Title);
                            }
                        }
                    }
                    if (Cmd == 2)
                    { 
                        if (String.IsNullOrEmpty(DisplayName))
                        {
                            notification("Vui lòng điền đầy đủ các trường yêu cầu", x.Title);
                        }
                        else
                        {
                            var item= DataProvider.Instance.DB.OBJECT_TYPE.Where(y => y.ID == SelectedItem.ID).SingleOrDefault();
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
            ObjectTypeList = new List<OBJECT_TYPE>(DataProvider.Instance.DB.OBJECT_TYPE);
            ObjectTypes = new ObservableCollection<ObjectType>();
            int i = 1;
            foreach(var item in ObjectTypeList)
            {
                ObjectType objectType = new ObjectType();
                objectType.STT = i;
                objectType.NAME = item.NAME;
                objectType.ID = item.ID;
                ObjectTypes.Add(objectType);
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
