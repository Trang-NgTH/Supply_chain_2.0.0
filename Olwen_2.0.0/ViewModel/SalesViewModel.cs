using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using Olwen_2._0._0.DataModel;
using Olwen_2._0._0.Model;
using Olwen_2._0._0.Repositories.Implements;
using Olwen_2._0._0.Repositories.Interfaces;
using Olwen_2._0._0.View.Components;
using Olwen_2._0._0.View.DialogsResult;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Olwen_2._0._0.ViewModel
{
    public class SalesViewModel:ViewModelBase
    {
        private IAsyncRepository<OrderHeader> oh_repo;
        private ObservableCollection<OrderHeader> _listOrder;
        private OrderHeader _orderSelected;
        private ObservableCollection<ProductODModel> _listOrderDetail;
        private string _cusName, _empName, _deiName, _orderDate, _discount, _subtotal;
        static DialogContent dc = new DialogContent();
        static DialogOk dialog = new DialogOk();
        private const string DialogHostId = "RootDialogHostId61";

        public DelegateCommand LoadedCommand
        {
            get;
            private set;
        }

        public DelegateCommand<int?> DeleteOrderCommand
        {
            get;
            private set;
        }

        public DelegateCommand AddNewOrder
        {
            get;
            private set;
        }

        public ObservableCollection<OrderHeader> ListOrder
        {
            get
            {
                return _listOrder;
            }

            set
            {
                _listOrder = value;
                RaisePropertyChanged();
            }
        }

        public OrderHeader OrderSelected
        {
            get
            {
                return _orderSelected;
            }

            set
            {
                _orderSelected = value;
                RaisePropertyChanged();
                if(value!=null)
                {
                    if(_orderSelected.OrderDetails!=null)
                    ListOrderDetail = new ObservableCollection<ProductODModel>(
                                                                            _orderSelected.OrderDetails.Select(
                                                                                c=> new ProductODModel()
                                                                                {
                                                                                    Name = c.Product.Name,
                                                                                    OrderQty = c.OrderQty,
                                                                                    ProductID = c.ProductId
                                                                                }
                                                                            ));

                    //if(_orderSelected.Customer!=null)
                    //    CusName = _orderSelected.Customer.Name;
                    //else 

                    //EmpName = _orderSelected.Employee.Name;
                    //DeiName = _orderSelected.Logistic.Name;
                    //OrderDate = _orderSelected.OrderDate.ToString();
                    if (_orderSelected.Discount != null)
                        Discount = _orderSelected.Discount.ToString();
                    else
                        Discount = "0";
                    decimal? subtotal = 0;
                    foreach(var i in _orderSelected.OrderDetails)
                    {
                        subtotal += i.OrderQty * i.Product.UnitOnOrder;
                    }
                    Subtotal = subtotal.ToString();
                }
            }
        }

        public ObservableCollection<ProductODModel> ListOrderDetail
        {
            get
            {
                return _listOrderDetail;
            }

            set
            {
                _listOrderDetail = value;
                RaisePropertyChanged();
            }
        }

        public string CusName
        {
            get
            {
                return _cusName;
            }

            set
            {
                _cusName = value;
                RaisePropertyChanged();
            }
        }

        public string EmpName
        {
            get
            {
                return _empName;
            }

            set
            {
                _empName = value;
                RaisePropertyChanged();
            }
        }

        public string DeiName
        {
            get
            {
                return _deiName;
            }

            set
            {
                _deiName = value;
                RaisePropertyChanged();
            }
        }

        public string OrderDate
        {
            get
            {
                return _orderDate;
            }

            set
            {
                _orderDate = value;
                RaisePropertyChanged();
            }
        }

        public string Discount
        {
            get
            {
                return _discount;
            }

            set
            {
                _discount = value;
                RaisePropertyChanged();
            }
        }

        public string Subtotal
        {
            get
            {
                return _subtotal;
            }

            set
            {
                _subtotal = value;
                RaisePropertyChanged();
            }
        }

        public SalesViewModel()
        {
            oh_repo = new BaseAsyncRepository<OrderHeader>();
            LoadedCommand = new DelegateCommand(CreateData);
            DeleteOrderCommand = new DelegateCommand<int?>(DeleteOrder);
            AddNewOrder = new DelegateCommand(CreateOrder);

            MessengerInstance.Register<OrderHeader>(this, Addsuccess);
        }

        private async void Addsuccess(OrderHeader obj)
        {
            try
            {
                if (obj != null)
                {
                    var newobj = await oh_repo.GetById(obj.OrderID);
                    ListOrder.Add(newobj);
                    dc = new DialogContent() { Content = "Thêm Thành Công", Tilte = "Thông Báo" };
                    dialog = new DialogOk() { DataContext = dc };
                    DialogHost.CloseDialogCommand.Execute(null, null);
                    await DialogHost.Show(dialog, DialogHostId);
                }
                else
                {
                    dc = new DialogContent() { Content = "Thêm Thất Bại", Tilte = "Thông Báo" };
                    dialog = new DialogOk() { DataContext = dc };
                    DialogHost.CloseDialogCommand.Execute(null, null);
                    await DialogHost.Show(dialog, DialogHostId);
                }
                
            }
            catch
            {
                dc.Content = "Có Lỗi";
                dc.Tilte = "Thông Báo";
                dialog = new DialogOk() { DataContext = dc };
                DialogHost.CloseDialogCommand.Execute(null, null);
                await DialogHost.Show(dialog, DialogHostId);
            }
        }

        private async void CreateOrder()
        {
            try
            {
                await DialogHost.Show(new OrderProfile(), DialogHostId);
            }
            catch
            {
                MessageBox.Show("Có Lỗi");
            }
        }

        private async void DeleteOrder(int? obj)
        {
            try
            {
                dc = new DialogContent() { Content = "Bạn muốn xóa hóa đơn này ?", Tilte = "Thông Báo" };
                var dialogYS = new DialogYesNo() { DataContext = dc };
                var result = (bool)await DialogHost.Show(dialogYS, DialogHostId);
                if (result)
                {
                    
                    if (await oh_repo.Remove((int)obj))
                    {
                        dc = new DialogContent() { Content = "Xóa Thành Công", Tilte = "Thông Báo" };
                        dialog = new DialogOk() { DataContext = dc };
                        DialogHost.CloseDialogCommand.Execute(null, null);
                        await DialogHost.Show(dialog, DialogHostId);
                        ListOrder.Remove(ListOrder.SingleOrDefault(t => t.OrderID == (int)obj));
                    }
                    else
                    {
                        dc = new DialogContent() { Content = "Xóa Thất Bại", Tilte = "Thông Báo" };
                        dialog = new DialogOk() { DataContext = dc };
                        DialogHost.CloseDialogCommand.Execute(null, null);
                        await DialogHost.Show(dialog, DialogHostId);
                    }
                }
            }
            catch
            {
                dc.Content = "Có Lỗi";
                dc.Tilte = "Thông Báo";
                dialog = new DialogOk() { DataContext = dc };
                DialogHost.CloseDialogCommand.Execute(null, null);
                await DialogHost.Show(dialog, DialogHostId);
            }
        }

        private async void CreateData()
        {
            ListOrder = new ObservableCollection<OrderHeader>(await oh_repo.GetAllAsync());
            OrderSelected = ListOrder.First();
        }
    }
}
