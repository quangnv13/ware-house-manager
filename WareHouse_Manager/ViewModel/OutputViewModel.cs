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
    public class OutputViewModel:BaseViewModel
    {
        #region Output
        private ObservableCollection<Outputs> _outputList;
        public ObservableCollection<Outputs> OutputList { get => _outputList; set { _outputList = value; OnPropertyChanged(); } }
        private DateTime _dateOutput;
        public DateTime DateOutput { get => _dateOutput; set { _dateOutput = value; OnPropertyChanged(); } }
        private List<Customerss> _customerList;
        public List<Customerss>CustomerList{ get=>_customerList; set {_customerList=value;OnPropertyChanged(); } }
        private int _currentCustomer;
        public int CurrentCustomer { get=>_currentCustomer; set {_currentCustomer=value;OnPropertyChanged(); } }
        private List<Status> _statusList;
        public List<Status> StatusList { get => _statusList; set { _statusList = value; OnPropertyChanged(); } }
        private int _currentStatus;
        public int CurrentStatus { get => _currentStatus; set { _currentStatus = value; OnPropertyChanged(); } }
        private bool _buttonDetailIsEnabled;
        public bool ButtonDetailIsEnabled { get=>_buttonDetailIsEnabled; set {_buttonDetailIsEnabled=value;OnPropertyChanged(); } }
        private int _outputCmd;
        public int OutputCmd
        {
            get => _outputCmd;
            set
            {
                _outputCmd = value;
                if (OutputCmd == 1 || OutputCmd == 2)
                {
                    OutputIsEnabled = false;
                    EnableEditOutput = true;
                    ButtonDetailIsEnabled2 = false;
                }
                else
                {
                    OutputIsEnabled = true;
                    EnableEditOutput = false;
                    ButtonDetailIsEnabled2 = true;
                }
                OnPropertyChanged();
            }
        }
        private string _searchID;
        public string SearchID { get => _searchID; set { _searchID = value; OnPropertyChanged(); } }
        private bool _enableEditOutput;
        public bool EnableEditOutput
        {
            get => _enableEditOutput;
            set
            {
                _enableEditOutput = value;
                if (EnableEditOutput == true)
                    OutputDetailIsEnabled = false;
                else
                    OutputDetailIsEnabled = true;
                OnPropertyChanged();
            }
        }
        private bool _outputIsEnabled;
        public bool OutputIsEnabled { get => _outputIsEnabled; set { _outputIsEnabled = value; OnPropertyChanged(); } }

        private Outputs _outputSelectedItem;
        public Outputs OutputSelectedItem
        {
            get => _outputSelectedItem;
            set
            {
                _outputSelectedItem = value;
                if (OutputSelectedItem != null)
                {
                    DateOutput = OutputSelectedItem.DATE_OUTPUT;
                    LoadOutputDetailDefault(OutputSelectedItem.ID);
                    if (OutputSelectedItem.STATUS == "Chờ thanh toán")
                        CurrentStatus = 0;
                    else if (OutputSelectedItem.STATUS == "Đã thanh toán")
                        CurrentStatus = 1;
                    else
                        CurrentStatus = -1;

                    CurrentCustomer = OutputSelectedItem.CUSTOMER;
                    DateOutput = OutputSelectedItem.DATE_OUTPUT;
                }
                OnPropertyChanged();
            }
        }
        public ICommand SearchOutputCommand { get; set; }
        public ICommand AddOutputCommand { get; set; }
        public ICommand EditOutputCommand { get; set; }
        public ICommand DelOutputCommand { get; set; }
        public ICommand SaveOutputCommand { get; set; }
        #endregion

        #region OutputDetail
        private ObservableCollection<OutputDetails> _outputDetailList;
        public ObservableCollection<OutputDetails> OutputDetailList { get => _outputDetailList; set { _outputDetailList = value; OnPropertyChanged(); } }
        private string _displayName;
        public string DisplayName { get => _displayName; set { _displayName = value; OnPropertyChanged(); } }
        private int _amount;
        public int Amount { get => _amount; set { _amount = value; ; OnPropertyChanged(); } }
        private List<Unit> _unitList;
        public List<Unit> UnitList { get => _unitList; set { _unitList = value; OnPropertyChanged(); } }
        private int _currentUnit;
        public int CurrentUnit { get => _currentUnit; set { _currentUnit = value; OnPropertyChanged(); } }
        private List<Suplier> _suplierList;
        public List<Suplier> SuplierList { get => _suplierList; set { _suplierList = value; OnPropertyChanged(); } }
        private int _currentSuplier;
        public int CurrentSuplier { get => _currentSuplier; set { _currentSuplier = value; OnPropertyChanged(); } }
        private List<ObjectType> _objectTypeList;
        public List<ObjectType> ObjectTypeList { get => _objectTypeList; set { _objectTypeList = value; OnPropertyChanged(); } }
        private int _currentObjectType;
        public int CurrentObjectType { get => _currentObjectType; set { _currentObjectType = value; OnPropertyChanged(); } }
        private List<Objectss> _objectList;
        public List<Objectss> ObjectList { get=>_objectList; set {_objectList=value;OnPropertyChanged(); } }
        private int _currentObject;
        public int CurrentObject
        {
            get =>_currentObject;
            set
            {
                _currentObject =value;
                if(CurrentObject>0)
                {
                    GetQuantity();
                }
                OnPropertyChanged();
            }
        }
        private string _quantity;
        public string Quantity { get=>_quantity; set {_quantity=value;OnPropertyChanged(); } }

        private int _outPrice;
        public int OutPrice { get => _outPrice; set { _outPrice = value; OnPropertyChanged(); } }
        private bool _enableEditOutputDetail;
        public bool EnableEditOutputDetail
        {
            get => _enableEditOutputDetail;
            set
            {
                _enableEditOutputDetail = value;
                if (EnableEditOutputDetail == true)
                    OutputIsEnabled = false;
                else
                    OutputIsEnabled = true;
                OnPropertyChanged();
            }
        }
        private bool _outputDetailIsEnabled;
        public bool OutputDetailIsEnabled { get => _outputDetailIsEnabled; set { _outputDetailIsEnabled = value; OnPropertyChanged(); } }
        private bool _buttonDetailIsEnabled2;
        public bool ButtonDetailIsEnabled2 { get=>_buttonDetailIsEnabled2; set {_buttonDetailIsEnabled2=value;OnPropertyChanged(); } }

        private int _outputDetailCmd;
        public int OutputDetailCmd
        {
            get => _outputDetailCmd;
            set
            {
                _outputDetailCmd = value;
                if (OutputDetailCmd == 1 || OutputDetailCmd == 2)
                {
                    EnableEditOutputDetail = true;
                    OutputDetailIsEnabled = false;
                    ButtonDetailIsEnabled = false;
                }
                else
                {
                    EnableEditOutputDetail = false;
                    OutputDetailIsEnabled = true;
                    ButtonDetailIsEnabled = true;
                }
                OnPropertyChanged();
            }
        }

        private OutputDetails _outputDetailSelectedItem;
        public OutputDetails OutputDetailSelectedItem
        {
            get => _outputDetailSelectedItem;
            set
            {
                _outputDetailSelectedItem = value;
                if (OutputDetailSelectedItem != null)
                {
                    Amount = (int)OutputDetailSelectedItem.AMOUNT;
                    CurrentObject =(int) OutputDetailSelectedItem.OBJECT_ID;
                }
                OnPropertyChanged();
            }
        }
        public ICommand AddOutputDetailCommand { get; set; }
        public ICommand EditOutputDetailCommand { get; set; }
        public ICommand DelOutputDetailCommand { get; set; }
        public ICommand SaveOutputDetailCommand { get; set; }
        public ICommand AllOutputDetailCommand { get; set; }

        #endregion
        public OutputViewModel()
        {
            LoadOutputDefault();
            LoadOutputDetailDefault();
            LoadStatusList();
            LoadUnitList();
            LoadSuplierList();
            LoadObjectTypeList();
            LoadCustomerList();
            LoadObjectList();

            #region OutputCommand
            SearchOutputCommand = new RelayCommand<Window>(
                x => true,
                x =>
                {
                    LoadOutputDefault();
                    if (String.IsNullOrEmpty(SearchID))
                    {
                        LoadOutputDefault();
                        LoadOutputDetailDefault();
                    }
                    else
                    {
                        OutputList = new ObservableCollection<Outputs>(OutputList.Where(y => y.ID.Contains(SearchID)));
                        //if(OutputList.Count==0)
                        //{
                        //    var findByCustomer = DataProvider.Instance.DB.OUTPUTs.Where(y => y.CUSTOMER.NAME.Contains(SearchID));
                        //    if(findByCustomer.Count()>0)
                        //    {
                        //        foreach(var item in findByCustomer)
                        //        {
                        //            Outputs output = new Outputs();
                        //            output.ID = item.ID;
                        //            output.DATE_OUTPUT = (DateTime)item.DATE_OUTPUT;
                        //            if (item.STATUS == 0)
                        //                output.STATUS = "Chờ thanh toán";
                        //            else
                        //                output.STATUS = "Đã thanh toán";
                        //            OutputList.Add(output);
                        //        }
                        //    }
                        //}
                    }
                });

            AddOutputCommand = new RelayCommand<Window>(
                x =>
                {
                    if (OutputCmd == 1 || OutputCmd == 2)
                        return false;
                    return true;
                },
                x =>
                {
                    DateOutput = DateTime.Now;
                    CurrentStatus = -1;
                    CurrentCustomer = -1;
                    OutputCmd = 1;
                });
            EditOutputCommand = new RelayCommand<Window>(
                x =>
                {
                    if (OutputSelectedItem == null || OutputCmd == 2 || OutputCmd == 1)
                        return false;
                    return true;
                },
                x =>
                {
                    OutputCmd = 2;
                });
            DelOutputCommand = new RelayCommand<Window>(
                x =>
                {
                    if (OutputSelectedItem == null || OutputCmd == 1 || OutputCmd == 2)
                        return false;
                    return true;
                },
                x =>
                {
                    MessageBoxResult boxResult = MessageBox.Show("Bạn có muốn xóa không!!", "Xóa mục này", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (boxResult == MessageBoxResult.Yes)
                    {
                        var output = DataProvider.Instance.DB.OUTPUT.Where(y => y.ID == OutputSelectedItem.ID).SingleOrDefault();
                        DataProvider.Instance.DB.OUTPUT.Remove(output);
                        DataProvider.Instance.DB.SaveChanges();
                        notification("Đã xóa!!", x.Title);
                    }
                    else
                    {
                        notification("Đã hủy thao tác", x.Title);
                    }
                    LoadOutputDefault();
                    LoadOutputDetailDefault();
                });
            SaveOutputCommand = new RelayCommand<Window>(
                x =>
                {
                    if (OutputCmd != 0)
                        return true;
                    return false;
                },
                x =>
                {
                    if (OutputCmd == 1)
                    {
                        if (CurrentStatus < 0 || CurrentCustomer<0)
                        {
                            notification("Vui lòng điền đầy đủ các trường yêu cầu", x.Title);
                        }
                        else
                        {
                            List<SqlParameter> parameters = new List<SqlParameter>();
                            parameters.Add(new SqlParameter("@DateOutput", String.Format("{0:MM/dd/yyyy}", DateOutput)));
                            parameters.Add(new SqlParameter("@CustomerID", CurrentCustomer));
                            parameters.Add(new SqlParameter("@Status", CurrentStatus));
                            DataProvider.Instance.DB.Database.ExecuteSqlCommand("INSERT dbo.Output (DATE_OUTPUT,CUSTOMER_ID, STATUS) VALUES(@DateOutput,@CustomerID,@Status)",parameters.ToArray());
                            notification("Đã thêm thành công", x.Title);
                        }
                    }
                    if (OutputCmd == 2)
                    {
                        if (CurrentStatus < 0||CurrentCustomer<0)
                        {
                            notification("Vui lòng điền đầy đủ các trường yêu cầu", x.Title);
                        }
                        else
                        {
                            var item = DataProvider.Instance.DB.OUTPUT.Where(y => y.ID == OutputSelectedItem.ID).SingleOrDefault();
                            item.DATE_OUTPUT = DateOutput;
                            item.CUSTOMER_ID = CurrentCustomer;
                            item.STATUS = CurrentStatus;
                            DataProvider.Instance.DB.SaveChanges();
                            notification("Đã sửa thành công", x.Title);
                        }
                    }
                    LoadOutputDefault();
                    LoadOutputDetailDefault();
                });
            #endregion

            #region OutputDetailCommand
            AllOutputDetailCommand = new RelayCommand<Window>(
                x => true,
                x =>
                {
                    LoadOutputDetailDefault();
                });
            AddOutputDetailCommand = new RelayCommand<Window>(
                x =>
                {
                    if (OutputDetailCmd == 1 || OutputDetailCmd == 2)
                        return false;
                    return true;
                },
                x =>
                {
                    if (OutputSelectedItem == null)
                    {
                        notification("Chưa chọn phiếu nhập", x.Title);
                    }
                    else
                    {
                        CurrentObject = -1;
                        Amount = 1;
                        OutputDetailCmd = 1;
                    }
                });
            EditOutputDetailCommand = new RelayCommand<Window>(
                x =>
                {
                    if (OutputDetailSelectedItem == null || OutputDetailCmd == 2 || OutputDetailCmd == 1)
                        return false;
                    return true;
                },
                x =>
                {
                    OutputDetailCmd = 2;
                });
            DelOutputDetailCommand = new RelayCommand<Window>(
                x =>
                {
                    if (OutputDetailSelectedItem == null || OutputDetailCmd == 1 || OutputDetailCmd == 2)
                        return false;
                    return true;
                },
                x =>
                {
                    MessageBoxResult boxResult = MessageBox.Show("Bạn có muốn xóa không!!", "Xóa mục này", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (boxResult == MessageBoxResult.Yes)
                    {
                        var outputDetail = DataProvider.Instance.DB.OUTPUT_DETAIL.Where(y => y.ID == OutputDetailSelectedItem.ID).SingleOrDefault();
                        DataProvider.Instance.DB.OUTPUT_DETAIL.Remove(outputDetail);
                        DataProvider.Instance.DB.SaveChanges();
                        notification("Đã xóa!!", x.Title);
                    }
                    else
                    {
                        notification("Đã hủy thao tác", x.Title);
                    }
                    LoadOutputDetailDefault(OutputSelectedItem.ID);
                    GetQuantity();
                });
            SaveOutputDetailCommand = new RelayCommand<Window>(
                x =>
                {
                    if (OutputDetailCmd != 0)
                        return true;
                    return false;
                },
                x =>
                {
                    if (OutputDetailCmd == 1)
                    {
                        if (CurrentObject < 0)
                        {
                            notification("Vui lòng điền đầy đủ các trường yêu cầu", x.Title);
                            goto breakif;
                        }
                        if (CheckQuanity() == true)
                        {
                            List<SqlParameter> parameters = new List<SqlParameter>();
                            parameters.Add(new SqlParameter("@OutputID", OutputSelectedItem.ID));
                            parameters.Add(new SqlParameter("@InputDetailID", CurrentObject));
                            parameters.Add(new SqlParameter("@Amount", Amount));

                            DataProvider.Instance.DB.Database.ExecuteSqlCommand(@"INSERT dbo.OUTPUT_DETAIL(OUTPUT_ID,INPUT_DETAIL_ID,AMOUNT) VALUES  ( @OutputID,@InputDetailID,@Amount)", parameters.ToArray());
                            notification("Đã thêm thành công", x.Title);
                        }
                    }
                    if (OutputDetailCmd == 2)
                    {
                        if (CurrentObject < 0)
                        {
                            notification("Vui lòng điền đầy đủ các trường yêu cầu", x.Title);
                            goto breakif;
                        }
                        else
                        {
                            if (CheckQuanity() == true)
                            {
                                var item = DataProvider.Instance.DB.OUTPUT_DETAIL.Where(y => y.ID == OutputDetailSelectedItem.ID).SingleOrDefault();
                                item.OUTPUT_ID = OutputSelectedItem.ID;
                                item.INPUT_DETAIL_ID = CurrentObject;
                                item.AMOUNT = Amount;
                                DataProvider.Instance.DB.SaveChanges();
                                notification("Đã sửa thành công", x.Title);
                            }
                        }
                    }
                    if (OutputSelectedItem != null)
                    {
                        LoadOutputDetailDefault(OutputSelectedItem.ID);
                    }
                    else
                    {
                        LoadOutputDetailDefault();
                    }
                breakif: //Skip notification
                    GetQuantity();
                });
            #endregion
        }
        void LoadOutputDefault()
        {
            OutputList = new ObservableCollection<Outputs>();
            var outputs = DataProvider.Instance.DB.OUTPUT;
            if (outputs != null)
            {
                foreach (var item in outputs)
                {
                    Outputs outputss = new Outputs();
                    outputss.ID = item.ID;
                    outputss.CUSTOMER =(int) item.CUSTOMER_ID;
                    outputss.DATE_OUTPUT = (DateTime)item.DATE_OUTPUT;
                    if (item.STATUS == 0)
                        outputss.STATUS = "Chờ thanh toán";
                    else if (item.STATUS == 1)
                        outputss.STATUS = "Đã thanh toán";
                    OutputList.Add(outputss);
                }
            }
            CurrentStatus = -1;
            OutputCmd = 0;
            ButtonDetailIsEnabled = true;
        }
        void LoadOutputDetailDefault(string outputID = null)
        {
            OutputDetailList = new ObservableCollection<OutputDetails>();
            if (outputID != null)
            {
                if (outputID!=null)
                {
                    var outputDetailList = DataProvider.Instance.DB.OUTPUT_DETAIL.Where(x => x.OUTPUT_ID == outputID);
                    foreach (var item in outputDetailList)
                    {
                        OutputDetails outputDetail = new OutputDetails();
                        outputDetail.ID = item.ID;
                        outputDetail.AMOUNT = (int)item.AMOUNT;
                        outputDetail.OBJECT_ID = (int)item.INPUT_DETAIL_ID;
                        outputDetail.NAME = DataProvider.Instance.DB.INPUT_DETAIL.Where(x => x.ID == item.INPUT_DETAIL_ID).SingleOrDefault().NAME;
                        outputDetail.SUPLIER = DataProvider.Instance.DB.SUPLIER.Where(x => x.ID == item.INPUT_DETAIL.SUPLIER_ID).SingleOrDefault().NAME;
                        outputDetail.UNIT = DataProvider.Instance.DB.UNIT.Where(x => x.ID == item.INPUT_DETAIL.UNIT_ID).SingleOrDefault().NAME;
                        outputDetail.OUT_PRICE = (int)DataProvider.Instance.DB.INPUT_DETAIL.Where(x => x.ID == item.INPUT_DETAIL_ID).SingleOrDefault().OUT_PRICE;
                        outputDetail.OBJECT_TYPE = DataProvider.Instance.DB.OBJECT_TYPE.Where(x => x.ID == item.INPUT_DETAIL.OBJECT_TYPE_ID).SingleOrDefault().NAME;
                        OutputDetailList.Add(outputDetail);
                    }
                }
            }
            else
            {
                OutputDetailList = new ObservableCollection<OutputDetails>();
                var outputDetailList = DataProvider.Instance.DB.OUTPUT_DETAIL;
                foreach (var item in outputDetailList)
                {
                    OutputDetails outputDetail = new OutputDetails();
                    outputDetail.ID = item.ID;
                    outputDetail.AMOUNT = (int)item.AMOUNT;
                    outputDetail.OBJECT_ID = (int)item.INPUT_DETAIL_ID;
                    outputDetail.NAME = DataProvider.Instance.DB.INPUT_DETAIL.Where(x => x.ID == item.INPUT_DETAIL_ID).SingleOrDefault().NAME;
                    outputDetail.SUPLIER = DataProvider.Instance.DB.SUPLIER.Where(x => x.ID == item.INPUT_DETAIL.SUPLIER_ID).SingleOrDefault().NAME;
                    outputDetail.UNIT = DataProvider.Instance.DB.UNIT.Where(x => x.ID == item.INPUT_DETAIL.UNIT_ID).SingleOrDefault().NAME;
                    outputDetail.OUT_PRICE = (int)DataProvider.Instance.DB.INPUT_DETAIL.Where(x => x.ID == item.INPUT_DETAIL_ID).SingleOrDefault().OUT_PRICE;
                    OutputDetailList.Add(outputDetail);
                }

            }
            Amount = 0;
            CurrentObject = -1;
            OutputDetailCmd = 0;
            ButtonDetailIsEnabled2 = true;
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
            foreach (var item in units)
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
            foreach (var item in supliers)
            {
                Suplier suplier = new Suplier();
                suplier.Id = item.ID;
                suplier.DisplayName = item.NAME;
                SuplierList.Add(suplier);
            }
        }
        void LoadCustomerList()
        {
            CustomerList = new List<Customerss>();
            var customers = DataProvider.Instance.DB.CUSTOMER;
            foreach(var item in customers)
            {
                Customerss customer = new Customerss();
                customer.Id = item.ID;
                customer.DisplayName=item.PHONE+" - "+item.NAME;
                CustomerList.Add(customer);
            }
        }
        void LoadObjectList()
        {
            ObjectList = new List<Objectss>();
            var objects= DataProvider.Instance.DB.INPUT_DETAIL;
            foreach(var item in objects)
            {
                Objectss objectss= new Objectss();
                objectss.Id = item.ID;
                objectss.DisplayName = item.ID + " - " + item.NAME;
                ObjectList.Add(objectss);
            }
        }
        void LoadObjectTypeList()
        {
            ObjectTypeList = new List<ObjectType>();
            var objectTypes = DataProvider.Instance.DB.OBJECT_TYPE;
            foreach (var item in objectTypes)
            {
                ObjectType objectType = new ObjectType();
                objectType.ID = item.ID;
                objectType.NAME = item.NAME;
                ObjectTypeList.Add(objectType);
            }
        }
        void GetQuantity()
        {
            int quantity = 0;
            var objectIn = DataProvider.Instance.DB.INPUT_DETAIL.Where(x=>x.ID==CurrentObject).SingleOrDefault();
            var objectOut = DataProvider.Instance.DB.OUTPUT_DETAIL.Where(x => x.INPUT_DETAIL_ID == CurrentObject);
            if (objectIn != null && objectOut != null)
            {
                quantity = (int)objectIn.AMOUNT;
                foreach (var item in objectOut)
                {
                    quantity = quantity - (int)item.AMOUNT;
                }
                Quantity = "Còn " + quantity + " " + objectIn.UNIT.NAME;
            }
            else
                Quantity = "--------";
        }
        bool CheckQuanity()
        {
            int quantity = 0;
            var objectIn = DataProvider.Instance.DB.INPUT_DETAIL.Where(x => x.ID == CurrentObject).SingleOrDefault();
            var objectOut = DataProvider.Instance.DB.OUTPUT_DETAIL.Where(x => x.INPUT_DETAIL_ID == CurrentObject);
            if (objectIn != null && objectOut != null)
            {
                quantity = (int)objectIn.AMOUNT;
                foreach (var item in objectOut)
                {
                    quantity = quantity - (int)item.AMOUNT;
                }
                if(Amount>quantity)
                {
                    notification("Số lượng xuất nhiều hơn số lượng còn!!", "Xuất kho");
                    return false;
                }
            }
            if(Amount<0)
            {
                notification("Số lượng xuất không hợp lệ","Xuất kho");
                return false;
            }
            return true;
        }
        void notification(string notification, string title)
        {
            NotificationUC notificationWindow = new NotificationUC(notification, "Thông báo -- " + title);
            notificationWindow.ShowDialog();
        }
    }
    public class Customerss
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
    }
    public class Objectss
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
    }
}
