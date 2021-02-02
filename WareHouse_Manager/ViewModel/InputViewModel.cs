using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WareHouse_Manager.Model;
using WareHouse_Manager.UC;

namespace WareHouse_Manager.ViewModel
{
    public class InputViewModel:BaseViewModel
    {
        #region Input
        private ObservableCollection<Inputs> _inputList;
        public ObservableCollection<Inputs> InputList { get=>_inputList; set {_inputList=value;OnPropertyChanged(); } }
        private DateTime _dateInput;
        public DateTime DateInput { get => _dateInput; set { _dateInput = value; OnPropertyChanged(); } }
        private List<Status> _statusList;
        public List<Status> StatusList { get => _statusList; set { _statusList = value; OnPropertyChanged(); } }
        private int _currentStatus;
        public int CurrentStatus { get => _currentStatus; set { _currentStatus = value; OnPropertyChanged(); } }
        private int _inputCmd;
        public int InputCmd
        {
            get =>_inputCmd;
            set
            {
                _inputCmd =value;
                if (InputCmd == 1 || InputCmd == 2)
                {
                    InputIsEnabled = false;
                    EnableEditInput = true;
                    ButtonDetailIsEnabled2 = false;
                }
                else
                {
                    InputIsEnabled = true;
                    EnableEditInput = false;
                    ButtonDetailIsEnabled2 = true;
                }
                OnPropertyChanged();
            }
        }
        private string _searchID;
        public string SearchID { get=>_searchID; set {_searchID=value;OnPropertyChanged(); } }
        private bool _enableEditInput;
        public bool EnableEditInput
        {
            get =>_enableEditInput;
            set
            {
                _enableEditInput =value;
                if (EnableEditInput == true)
                    InputDetailIsEnabled = false;
                else
                    InputDetailIsEnabled = true;
                OnPropertyChanged();
            }
        }
        private bool _inputIsEnabled;
        public bool InputIsEnabled { get=>_inputIsEnabled; set { _inputIsEnabled = value; OnPropertyChanged(); } }
        private bool _buttonDetailIsEnabled;
        public bool ButtonDetailIsEnabled { get=>_buttonDetailIsEnabled; set {_buttonDetailIsEnabled=value;OnPropertyChanged(); } }

        private Inputs _inputSelectedItem;
        public Inputs InputSelectedItem
        {
            get =>_inputSelectedItem;
            set
            {
                _inputSelectedItem =value;
                if (InputSelectedItem != null)
                {
                    DateInput = InputSelectedItem.DATE_INPUT;
                    LoadInputDetailDefault(InputSelectedItem.ID);
                    if (InputSelectedItem.STATUS == "Chờ thanh toán")
                        CurrentStatus = 0;
                    else if (InputSelectedItem.STATUS == "Đã thanh toán")
                        CurrentStatus = 1;
                    else
                        CurrentStatus = -1;
                }
                OnPropertyChanged();
            }
        }
        public ICommand SearchInputCommand { get; set; }
        public ICommand AddInputCommand { get; set; }
        public ICommand EditInputCommand { get; set; }
        public ICommand DelInputCommand { get; set; }
        public ICommand SaveInputCommand { get; set; }
        #endregion

        #region InputDetail
        private ObservableCollection<INPUT_DETAIL> _inputDetailList;
        public ObservableCollection<INPUT_DETAIL> InputDetailList { get => _inputDetailList; set { _inputDetailList = value; OnPropertyChanged(); } }
        private string _displayName;
        public string DisplayName { get=>_displayName; set {_displayName=value ; OnPropertyChanged(); } }
        private int _amount;
        public int Amount { get=>_amount; set {_amount=value; ; OnPropertyChanged(); } }
        private List<Unit> _unitList;
        public List<Unit> UnitList { get=>_unitList; set {_unitList=value;OnPropertyChanged(); } }
        private int _currentUnit;
        public int CurrentUnit { get=>_currentUnit; set {_currentUnit=value; OnPropertyChanged(); } }
        private List<Suplier> _suplierList;
        public List<Suplier> SuplierList { get=>_suplierList; set { _suplierList = value; OnPropertyChanged(); } }
        private int _currentSuplier;
        public int CurrentSuplier { get=>_currentSuplier; set { _currentSuplier = value; OnPropertyChanged(); } }
        private int _inPrice;
        public int InPrice { get=>_inPrice; set { _inPrice = value; OnPropertyChanged(); } }
        private int _outPrice;
        public int OutPrice { get => _outPrice; set { _outPrice = value; OnPropertyChanged(); } }
        private bool _enableEditInputDetail;
        public bool EnableEditInputDetail
        {
            get => _enableEditInputDetail;
            set { _enableEditInputDetail = value;
                if (EnableEditInputDetail == true)
                    InputIsEnabled = false;
                else
                    InputIsEnabled = true;
                OnPropertyChanged();
            }
        }
        private bool _inputDetailIsEnabled;
        public bool InputDetailIsEnabled { get=>_inputDetailIsEnabled; set {_inputDetailIsEnabled=value; OnPropertyChanged(); } }
        private bool _buttonDetailIsEnabled2;
        public bool ButtonDetailIsEnabled2 { get=>_buttonDetailIsEnabled2; set {_buttonDetailIsEnabled2=value;OnPropertyChanged(); } }
        private List<ObjectType> _objectTypeList;
        public List<ObjectType> ObjectTypeList { get=>_objectTypeList; set {_objectTypeList=value; OnPropertyChanged(); } }
        private int _currentObjectType;
        public int CurrentObjectType { get=>_currentObjectType; set {_currentObjectType=value;OnPropertyChanged(); } }

        private int _inputDetailCmd;
        public int InputDetailCmd
        {
            get =>_inputDetailCmd;
            set
            {
                _inputDetailCmd =value;
                if (InputDetailCmd == 1 || InputDetailCmd == 2)
                {
                    EnableEditInputDetail = true;
                    InputDetailIsEnabled = false;
                    ButtonDetailIsEnabled = false;
                }
                else
                {
                    EnableEditInputDetail = false;
                    InputDetailIsEnabled = true;
                    ButtonDetailIsEnabled = true;
                }
                OnPropertyChanged();
            }
        }

        private INPUT_DETAIL _inputDetailSelectedItem;
        public INPUT_DETAIL InputDetailSelectedItem
        {
            get =>_inputDetailSelectedItem;
            set
            {
                _inputDetailSelectedItem =value;
                if(InputDetailSelectedItem!=null)
                {
                    DisplayName = InputDetailSelectedItem.NAME;
                    Amount = (int)InputDetailSelectedItem.AMOUNT;
                    CurrentUnit =(int) InputDetailSelectedItem.UNIT_ID;
                    CurrentSuplier = (int)InputDetailSelectedItem.SUPLIER_ID;
                    InPrice = (int)InputDetailSelectedItem.IN_PRICE;
                    OutPrice = (int)InputDetailSelectedItem.OUT_PRICE;
                    CurrentObjectType = (int)InputDetailSelectedItem.OBJECT_TYPE_ID;
                }
                OnPropertyChanged();
            }
        }
        public ICommand AddInputDetailCommand { get; set; }
        public ICommand EditInputDetailCommand { get; set; }
        public ICommand DelInputDetailCommand { get; set; }
        public ICommand SaveInputDetailCommand { get; set; }
        public ICommand AllInputDetailCommand { get; set; }

        #endregion
        public InputViewModel()
        {
            LoadInputDefault();
            LoadInputDetailDefault();
            LoadStatusList();
            LoadUnitList();
            LoadSuplierList();
            LoadObjectTypeList();

            InputDetailCmd = 0;

            #region InputCommand
            SearchInputCommand = new RelayCommand<Window>(
                x => true, 
                x => 
                {
                    LoadInputDefault();
                    if (String.IsNullOrEmpty(SearchID))
                    {
                        LoadInputDefault();
                    }
                    else
                    {
                        InputList =new ObservableCollection<Inputs>(InputList.Where(y => y.ID.Contains(SearchID)));
                    }
                });

            AddInputCommand = new RelayCommand<Window>(
                x =>
                {
                    if (InputCmd == 1 || InputCmd == 2)
                        return false;
                    return true;
                },
                x =>
                {
                    DateInput = DateTime.Now;
                    CurrentStatus = -1;
                    InputCmd = 1;
                });
            EditInputCommand = new RelayCommand<Window>(
                x =>
                {
                    if (InputSelectedItem == null || InputCmd == 2 || InputCmd == 1)
                        return false;
                    return true;
                },
                x =>
                {
                    InputCmd = 2;
                });
            DelInputCommand = new RelayCommand<Window>(
                x =>
                {
                    if (InputSelectedItem == null || InputCmd == 1 || InputCmd == 2)
                        return false;
                    return true;
                },
                x =>
                {
                    MessageBoxResult boxResult = MessageBox.Show("Bạn có muốn xóa không!!", "Xóa mục này", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (boxResult == MessageBoxResult.Yes)
                    {
                        var input = DataProvider.Instance.DB.INPUT.Where(y => y.ID == InputSelectedItem.ID).SingleOrDefault();
                        DataProvider.Instance.DB.INPUT.Remove(input);
                        DataProvider.Instance.DB.SaveChanges();
                        notification("Đã xóa!!", x.Title);
                    }
                    else
                    {
                        notification("Đã hủy thao tác", x.Title);
                    }
                    LoadInputDefault();
                    LoadInputDetailDefault();
                });
            SaveInputCommand = new RelayCommand<Window>(
                x =>
                {
                    if (InputCmd != 0)
                        return true;
                    return false;
                },
                x =>
                {
                    if (InputCmd == 1)
                    {
                        if (CurrentStatus < 0)
                        {
                            notification("Vui lòng điền đầy đủ các trường yêu cầu", x.Title);
                        }
                        else
                        {
                            DataProvider.Instance.DB.Database.ExecuteSqlCommand("INSERT dbo.INPUT (DATE_INPUT, STATUS) VALUES('" + String.Format("{0:MM/dd/yyyy}", DateInput) + "'," + CurrentStatus + ")");
                            notification("Đã thêm thành công", x.Title);
                        }
                    }
                    if (InputCmd == 2)
                    {
                        if (CurrentStatus < 0)
                        {
                            notification("Vui lòng điền đầy đủ các trường yêu cầu", x.Title);
                        }
                        else
                        {
                            var item = DataProvider.Instance.DB.INPUT.Where(y => y.ID == InputSelectedItem.ID).SingleOrDefault();
                            item.DATE_INPUT = DateInput;
                            item.STATUS = CurrentStatus;
                            DataProvider.Instance.DB.SaveChanges();
                            notification("Đã sửa thành công", x.Title);
                        }
                    }
                    LoadInputDefault();
                    LoadInputDetailDefault();
                });
            #endregion

            #region InputDetailCommand
            AllInputDetailCommand = new RelayCommand<Window>(
                x => true,
                x =>
                {
                    LoadInputDetailDefault();
                });
            AddInputDetailCommand = new RelayCommand<Window>(
                x =>
                {
                if (InputDetailCmd == 1 || InputDetailCmd == 2)
                        return false;
                    return true;
                },
                x =>
                {
                    if (InputSelectedItem == null)
                    {
                        notification("Chưa chọn phiếu nhập", x.Title);
                    }
                    else
                    {
                        CurrentSuplier = -1;
                        CurrentUnit = -1;
                        CurrentObjectType = -1;
                        DisplayName = "";
                        Amount = 1;
                        InPrice = 1;
                        OutPrice = 1;
                        InputDetailCmd = 1;
                    }
                });
            EditInputDetailCommand = new RelayCommand<Window>(
                x =>
                {
                    if (InputDetailSelectedItem == null || InputDetailCmd == 2 || InputDetailCmd == 1)
                        return false;
                    return true;
                },
                x =>
                {
                    InputDetailCmd = 2;
                });
            DelInputDetailCommand = new RelayCommand<Window>(
                x =>
                {
                    if (InputDetailSelectedItem == null || InputDetailCmd == 1 || InputDetailCmd == 2)
                        return false;
                    return true;
                },
                x =>
                {
                    MessageBoxResult boxResult = MessageBox.Show("Bạn có muốn xóa không!!", "Xóa mục này", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (boxResult == MessageBoxResult.Yes)
                    {
                        var inputDetail = DataProvider.Instance.DB.INPUT_DETAIL.Where(y => y.ID == InputDetailSelectedItem.ID).SingleOrDefault();
                        DataProvider.Instance.DB.INPUT_DETAIL.Remove(inputDetail);
                        DataProvider.Instance.DB.SaveChanges();
                        notification("Đã xóa!!", x.Title);
                    }
                    else
                    {
                        notification("Đã hủy thao tác", x.Title);
                    }
                    if (InputSelectedItem != null)
                    {
                        LoadInputDetailDefault(InputSelectedItem.ID);
                    }
                    else
                    {
                        LoadInputDetailDefault();
                    }
                });
            SaveInputDetailCommand = new RelayCommand<Window>(
                x =>
                {
                    if (InputDetailCmd != 0)
                        return true;
                    return false;
                },
                x =>
                {
                    int check = 0;
                    if (InputDetailCmd == 1)
                    {
                        if (CurrentSuplier < 0 || CurrentUnit<0||CurrentObjectType<0 ||String.IsNullOrEmpty(DisplayName))
                        {
                            notification("Vui lòng điền đầy đủ các trường yêu cầu", x.Title);
                            check = 1;
                            goto breakif;
                        }
                        if(Amount<1||InPrice<1||OutPrice<1)
                        {
                            notification("Vui giá trị lớn hơn 1", x.Title);
                            check = 1;
                        }
                        breakif:
                        if(check==0)
                        {
                            List<SqlParameter> parameters = new List<SqlParameter>();
                            parameters.Add(new SqlParameter("@InputID", InputSelectedItem.ID));
                            parameters.Add(new SqlParameter("@ObjectTypeID", CurrentObjectType));
                            parameters.Add(new SqlParameter("@Name", DisplayName));
                            parameters.Add(new SqlParameter("@SuplierID", CurrentSuplier));
                            parameters.Add(new SqlParameter("@UnitID", CurrentUnit));
                            parameters.Add(new SqlParameter("@InPrice", InPrice));
                            parameters.Add(new SqlParameter("@OutPrice", OutPrice));
                            parameters.Add(new SqlParameter("@Amount", Amount));

                            DataProvider.Instance.DB.Database.ExecuteSqlCommand(@"INSERT dbo.INPUT_DETAIL(INPUT_ID,OBJECT_TYPE_ID,NAME,SUPLIER_ID,UNIT_ID,AMOUNT,IN_PRICE,OUT_PRICE) VALUES  ( @InputID,@ObjectTypeID,@Name,@SuplierID,@UnitID,@Amount,@InPrice,@OutPrice)", parameters.ToArray());
                            notification("Đã thêm thành công", x.Title);
                        }
                        check = 0;
                    }
                    if (InputDetailCmd == 2)
                    {
                        var item = DataProvider.Instance.DB.INPUT_DETAIL.Where(y => y.ID == InputDetailSelectedItem.ID).SingleOrDefault();
                        item.NAME = DisplayName;
                        item.OBJECT_TYPE_ID = CurrentObjectType;
                        item.SUPLIER_ID = CurrentSuplier;
                        item.UNIT_ID = CurrentUnit;
                        item.IN_PRICE = InPrice;
                        item.OUT_PRICE = OutPrice;
                        item.AMOUNT = Amount;
                        DataProvider.Instance.DB.SaveChanges();
                        notification("Đã sửa thành công", x.Title);
                    }
                    if (InputSelectedItem != null)
                    {
                        LoadInputDetailDefault(InputSelectedItem.ID);
                    }
                    else
                    {
                        LoadInputDetailDefault();
                    }
                });
            #endregion
        }
        void LoadInputDefault()
        {
            InputList = new ObservableCollection<Inputs>();
            var inputs = DataProvider.Instance.DB.INPUT;
            if (inputs != null)
            {
                foreach (var item in inputs)
                {
                    Inputs inputss = new Inputs();
                    inputss.ID = item.ID;
                    inputss.DATE_INPUT = (DateTime)item.DATE_INPUT;
                    if (item.STATUS == 0)
                        inputss.STATUS = "Chờ thanh toán";
                    else if (item.STATUS == 1)
                        inputss.STATUS = "Đã thanh toán";
                    InputList.Add(inputss);
                }
            }
            CurrentStatus = -1;
            InputCmd = 0;
            ButtonDetailIsEnabled = true;
        }
        void LoadInputDetailDefault(string InputID = null)
        {
            if (InputID != null)
            {
                InputDetailList = new ObservableCollection<INPUT_DETAIL>(DataProvider.Instance.DB.INPUT_DETAIL.Where(x => x.INPUT_ID == InputID));
            }
            else
            {
                InputDetailList = new ObservableCollection<INPUT_DETAIL>(DataProvider.Instance.DB.INPUT_DETAIL);
            }
            CurrentSuplier = -1;
            CurrentUnit = -1;
            CurrentObjectType = -1;
            InputDetailCmd = 0;
            ButtonDetailIsEnabled2 = true ; 
        }
        void LoadStatusList()
        {
            StatusList = new List<Status>();
            StatusList.Add(new Status() { DisplayName = "Chờ thanh toán", Value = 0 });
            StatusList.Add(new Status() { DisplayName = "Đã thanh toán", Value = 1 });
        }
        void LoadUnitList()
        {
            UnitList = new List<Unit>();
            var units = DataProvider.Instance.DB.UNIT;
            foreach(var item in units)
            {
                Unit unit = new Unit();
                unit.Id = item.ID;
                unit.DisplayName = item.NAME;
                UnitList.Add(unit);
            }
        }
        void LoadSuplierList()
        {
            SuplierList = new List<Suplier>();
            var supliers = DataProvider.Instance.DB.SUPLIER;
            foreach(var item in supliers)
            {
                Suplier suplier = new Suplier();
                suplier.Id = item.ID;
                suplier.DisplayName = item.NAME;
                SuplierList.Add(suplier);
            }
        }
        void LoadObjectTypeList()
        {
            ObjectTypeList = new List<ObjectType>();
            var objectTypes = DataProvider.Instance.DB.OBJECT_TYPE;
            foreach(var item in objectTypes)
            {
                ObjectType objectType = new ObjectType();
                objectType.ID = item.ID;
                objectType.NAME = item.NAME;
                ObjectTypeList.Add(objectType);
            }
        }

        void notification(string notification, string title)
        {
            NotificationUC notificationWindow = new NotificationUC(notification, "Thông báo -- " + title);
            notificationWindow.ShowDialog();
        }
    }
    public class Status
    {
        public string DisplayName { get; set; }
        public int Value { get; set; }
    }
    public class Unit
    {
        public string DisplayName { get; set; }
        public int Id { get; set; }
    }
    public class Suplier
    {
        public string DisplayName { get; set; }
        public int Id { get; set; }
    }
}
