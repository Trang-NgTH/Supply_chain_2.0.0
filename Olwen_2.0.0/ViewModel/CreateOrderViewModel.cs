using GalaSoft.MvvmLight;
using Olwen_2._0._0.Model;
using Olwen_2._0._0.DependencyInjection;
using Olwen_2._0._0.Repositories.Implements;
using Olwen_2._0._0.Repositories.Interfaces;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Olwen_2._0._0.View.DialogsResult;
using MaterialDesignThemes.Wpf;
using Olwen_2._0._0.DataModel;
using GalaSoft.MvvmLight.Messaging;

namespace Olwen_2._0._0.ViewModel
{
    public class CreateOrderViewModel:ViewModelBase
    {
        private IAsyncRepository<Customer> cus_repo;
        private IAsyncRepository<Logistic> log_repo;
        private IProductRepository pro_repo;
        private IAsyncRepository<Store> store_repo;
        private IAsyncRepository<OrderHeader> oh_repo;
        private ObservableCollection<int> _listDis;
        private int _selectedDis;
        private int? _restQty;
        private ObservableCollection<Customer> _listCus;
        private Customer _selectedCus;
        private ObservableCollection<Logistic> _listLog;
        private Logistic _selectedLog;
        private ObservableCollection<Product> _listPro;
        private Product _selectedPro;
        private ObservableCollection<Store> _listStore;
        private Store _selectedSto;
        private string _qty;
        static DialogContent dc = new DialogContent();
        static DialogOk dialog = new DialogOk();
        private const string DialogHostId = "RootDialogHostsml";
        private ObservableCollection<ProductODModel> _listOD;
        private double _subtotal;
        private double _totalDue;
        private string _employeeName;

        public DelegateCommand<int?> DeleteProODCommand
        {
            get;
            private set;
        }

        public DelegateCommand<int?> InsertToCart
        {
            get;
            private set;
        }

        public DelegateCommand CreateOrderCommand
        {
            get;
            private set;
        }

        public ObservableCollection<Customer> ListCus
        {
            get
            {
                return _listCus;
            }

            set
            {
                _listCus = value;
                RaisePropertyChanged();
            }
        }

        public Customer SelectedCus
        {
            get
            {
                return _selectedCus;
            }

            set
            {
                _selectedCus = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand LoadingCommand
        {
            get;
            private set;
        }

        public ObservableCollection<Logistic> ListLog
        {
            get
            {
                return _listLog;
            }

            set
            {
                _listLog = value;
                RaisePropertyChanged();
            }
        }

        public Logistic SelectedLog
        {
            get
            {
                return _selectedLog;
            }

            set
            {
                _selectedLog = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Product> ListPro
        {
            get
            {
                return _listPro;
            }

            set
            {
                _listPro = value;
                RaisePropertyChanged();
            }
        }

        public Product SelectedPro
        {
            get
            {
                return _selectedPro;
            }

            set
            {
                _selectedPro = value;
                RaisePropertyChanged();
                if(_selectedPro!=null)
                {
                    ListStore = new ObservableCollection<Store>(pro_repo.GetAllStoreByProductID(_selectedPro.ProductID));
                    RestQty = null;
                }
            }
        }

        public ObservableCollection<Store> ListStore
        {
            get
            {
                return _listStore;
            }

            set
            {
                _listStore = value;
                RaisePropertyChanged();
            }
        }

        public Store SelectedSto
        {
            get
            {
                return _selectedSto;
            }

            set
            {
                _selectedSto = value;
                RaisePropertyChanged();
                if(_selectedSto!=null)
                {
                    RestQty = _selectedSto.StoreDetails.SingleOrDefault(t => t.Product.ProductID == SelectedPro.ProductID).Quantity;
                }
            }
        }

        public ObservableCollection<int> ListDis
        {
            get
            {
                return _listDis;
            }

            set
            {
                _listDis = value;
                RaisePropertyChanged();
            }
        }

        public int SelectedDis
        {
            get
            {
                return _selectedDis;
            }

            set
            {
                _selectedDis = value;
                RaisePropertyChanged();
                TotalDue = _subtotal * (1 - (_selectedDis * 1.0 / 100));
            }
        }

        public int? RestQty
        {
            get
            {
                return _restQty;
            }

            set
            {
                _restQty = value;
                RaisePropertyChanged();
            }
        }

        public string Qty
        {
            get
            {
                return _qty;
            }

            set
            {
                _qty = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ProductODModel> ListOD
        {
            get
            {
                return _listOD;
            }

            set
            {
                _listOD = value;
                RaisePropertyChanged("ListOD");
            }
        }

        public double Subtotal
        {
            get
            {
                return _subtotal;
            }

            set
            {
                _subtotal = value;
                RaisePropertyChanged();
                TotalDue = _subtotal * (1-(SelectedDis*1.0/100));
            }
        }

        public double TotalDue
        {
            get
            {
                return _totalDue;
            }

            set
            {
                _totalDue = value;
                RaisePropertyChanged();
            }
        }

        public string EmployeeName
        {
            get
            {
                return _employeeName;
            }

            set
            {
                _employeeName = value;
                RaisePropertyChanged();
            }
        }

        public CreateOrderViewModel()
        {
            EmployeeName = StateLogin.AccountLogin.Username;

            LoadingCommand = new DelegateCommand(CreateData);
            InsertToCart = new DelegateCommand<int?>(InsertIntoCart);
            DeleteProODCommand = new DelegateCommand<int?>(DeleteProOD);
            CreateOrderCommand = new DelegateCommand(CreateOrder);

            oh_repo = new BaseAsyncRepository<OrderHeader>();

            ListOD = new ObservableCollection<ProductODModel>();
            Subtotal = 0;
            TotalDue = 0;
            ListDis = new ObservableCollection<int>();
            cus_repo = new BaseAsyncRepository<Customer>();
            pro_repo = new ProductRepository();
            log_repo = new BaseAsyncRepository<Logistic>();
            store_repo = new BaseAsyncRepository<Store>();


            for (int i = 0; i <= 100; i++)
            {
                ListDis.Add(i);
            }

        }

        private async void CreateOrder()
        {
            try
            {
                if(ListOD.Count()>0)
                {

                    var newOrder = new OrderHeader();

                    if (SelectedCus != null)
                        newOrder.CusID = SelectedCus.CusID;

                    newOrder.Discount = SelectedDis;

                    if (SelectedLog != null)
                        newOrder.LogId = SelectedLog.LogID;

                    newOrder.OrderDate = DateTime.Now;

                    foreach(var i in ListOD)
                    {
                        newOrder.OrderDetails.Add(new OrderDetail()
                        {
                            ProductId = i.ProductID,
                            OrderQty = i.OrderQty,
                            UnitPrice =  (decimal)i.UnitPrice,
                            StoreID = i.StoreID
                        });
                    }

                    var result = await oh_repo.Add(newOrder);

                    MessengerInstance.Send<OrderHeader>(result);
                }
            }
            catch
            {
                MessageBox.Show("Có Lỗi");
            }
        }


        private async void DeleteProOD(int? obj)
        {
            try
            {
                dc = new DialogContent() { Content = "Bạn muốn sản phẩm viên này ?", Tilte = "Thông Báo" };
                var dialogYS = new DialogYesNo() { DataContext = dc };
                var result = (bool)await DialogHost.Show(dialogYS, DialogHostId);
                if (result)
                {
                    if (obj != null)
                    {
                        var item = ListOD.SingleOrDefault(t => t.ProductID == (int)obj);
                        ListOD.Remove(item);
                        Subtotal -= (double)item.UnitPrice * (int)item.OrderQty;

                    }
                }
            }
            catch 
            {
                MessageBox.Show("Có Lỗi", "Thông Báo",MessageBoxButton.OK,MessageBoxImage.Warning);
            }
        }

        private async void InsertIntoCart(int? obj)
        {
            try
            {
                if (obj != null)
                {
                    var item = ListPro.SingleOrDefault(t => t.ProductID == (int)obj);
                    if (item != null)
                    {
                        if (!string.IsNullOrEmpty(Qty) && Qty.IsNum())
                        {
                            if(ListOD.SingleOrDefault(t=>t.ProductID==(int)obj)==null)
                            {
                                if (Convert.ToInt32(Qty) <= RestQty && RestQty >= 0)
                                {
                                    var ProOD = new ProductODModel()
                                    {
                                        ProductID = _selectedPro.ProductID,
                                        Name = _selectedPro.Name,
                                    };
                                    ProOD.OrderQty = Convert.ToInt32(Qty);
                                    ProOD.UnitPrice = (double)_selectedPro.UnitOnOrder;
                                    ProOD.StoreID = SelectedSto.StoreID;
                                    Subtotal += (double)_selectedPro.UnitOnOrder * Convert.ToInt32(Qty);
                                    ListOD.Add(ProOD);
                                }
                                else
                                {
                                    MessageBox.Show("số lượng phải nhỏ hơn hoặc bằng số lượng hiện tại", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Đã Tồn Tại Trong Giỏ Hàng", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Có Lỗi - Số Lượng Phải Là Số", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Có Lỗi", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



        //paralell async
        private void CreateData()
        {
            List<Task> tasks = new List<Task>();

            tasks.Add(new Task(async () => { ListCus = new ObservableCollection<Customer>(await cus_repo.GetAllAsync()); }));

            tasks.Add(new Task(async () => { ListLog = new ObservableCollection<Logistic>(await log_repo.GetAllAsync()); }));

            tasks.Add(new Task(async () => { ListPro = new ObservableCollection<Product>(await pro_repo.GetAllAsync()); }));


            tasks.ForEach(a => a.Start());
        }
    }
}
