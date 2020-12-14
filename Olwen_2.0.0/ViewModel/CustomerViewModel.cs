using GalaSoft.MvvmLight;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Olwen_2._0._0.DependencyInjection;
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
using System.Windows.Media.Imaging;

namespace Olwen_2._0._0.ViewModel
{
    public class CustomerViewModel : ViewModelBase
    {
        private IAsyncRepository<Customer> customer_repo;
        private const string DialogHostId = "RootDialogHost1w";
        private ObservableCollection<Customer> _listCustomer;
        private ObservableCollection<string> _listKinds;
        private ObservableCollection<OrderHeader> _listOCus;
        private ObservableCollection<OrderDetail> _listODCus;
        private OrderHeader _selectedOCus;
        private bool _isB, _isG;
        private string _cusID;
        private string _sKey, _seaKey;
        private Customer _selectedCus;
        private string _name, _address, _phone, _email, _kind;
        private BitmapImage _imgCus;
        private ObservableCollection<string> _listKey;
        static DialogContent dc = new DialogContent();
        static DialogOk dialog = new DialogOk();
        private OpenFileDialog Op = new OpenFileDialog();


        public bool IsB
        {
            get
            {
                return _isB;
            }

            set
            {
                _isB = value;
                RaisePropertyChanged();
            }
        }

        public bool IsG
        {
            get
            {
                return _isG;
            }

            set
            {
                _isG = value;

                RaisePropertyChanged();
            }
        }

        public string CusID
        {
            get
            {
                return _cusID;
            }

            set
            {
                _cusID = value;

                RaisePropertyChanged();
            }
        }

        public string SKey
        {
            get
            {
                return _sKey;
            }

            set
            {
                _sKey = value;
                RaisePropertyChanged();
            }
        }

        public string SeaKey
        {
            get
            {
                return _seaKey;
            }

            set
            {
                _seaKey = value;
                RaisePropertyChanged();
                if (!string.IsNullOrEmpty(_seaKey))
                {
                    if (!string.IsNullOrEmpty(SKey))
                    {
                        if (ListKey.IndexOf(SKey) == 0)
                        {
                            if (SeaKey.IsNum())
                            {
                                ListCustomer = new ObservableCollection<Customer>(customer_repo.GetFilter(t => t.CusID == Convert.ToInt32(SeaKey)));
                            }
                        }
                        else
                        {
                            ListCustomer = new ObservableCollection<Customer>(customer_repo.GetFilter(t => t.Name.Contains(SeaKey)));
                        }
                    }
                }
                else if (_seaKey == null)
                    ListCustomer = new ObservableCollection<Customer>(customer_repo.GetAll());
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
                if (_selectedCus != null)
                {
                    ListOCus = new ObservableCollection<OrderHeader>(SelectedCus.OrderHeaders);
                    ShowListOrder();
                }
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public string Address
        {
            get
            {
                return _address;
            }

            set
            {
                _address = value;
                RaisePropertyChanged();
            }
        }

        public string Phone
        {
            get
            {
                return _phone;
            }

            set
            {
                _phone = value;
                RaisePropertyChanged();
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                _email = value;
                RaisePropertyChanged();
            }
        }

        public string Kind
        {
            get
            {
                return _kind;
            }

            set
            {
                _kind = value;
                RaisePropertyChanged();
            }
        }

        public BitmapImage ImgCus
        {
            get
            {
                return _imgCus;
            }

            set
            {
                _imgCus = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> ListKey
        {
            get
            {
                return _listKey;
            }

            set
            {
                _listKey = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Customer> ListCustomer
        {
            get
            {
                return _listCustomer;
            }

            set
            {
                _listCustomer = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> ListKinds
        {
            get
            {
                return _listKinds;
            }

            set
            {
                _listKinds = value;
                RaisePropertyChanged();
            }
        }

        public OrderHeader SelectedOCus
        {
            get
            {
                return _selectedOCus;
            }

            set
            {
                _selectedOCus = value;
                RaisePropertyChanged();
                if (SelectedOCus != null)
                {
                    ListODCus = new ObservableCollection<OrderDetail>(_selectedOCus.OrderDetails);
                }
            }
        }

        public DelegateCommand LoadedCommand
        {
            get;
            private set;
        }

        public DelegateCommand<int?> DeleteCusCommand
        {
            get;
            private set;
        }

        public DelegateCommand<int?> ShowInfoCommand
        {
            get;
            private set;
        }

        public DelegateCommand AddCusCommand
        {
            get;
            private set;
        }

        public DelegateCommand RefreshCommand
        {
            get;
            private set;
        }

        public DelegateCommand<string> SubmitCommand
        {
            get;
            private set;
        }

        public DelegateCommand ChoseImages
        {
            get;
            private set;
        }

        public ObservableCollection<OrderHeader> ListOCus
        {
            get
            {
                return _listOCus;
            }

            set
            {
                _listOCus = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<OrderDetail> ListODCus
        {
            get
            {
                return _listODCus;
            }

            set
            {
                _listODCus = value;
                RaisePropertyChanged();
            }
        }

        public CustomerViewModel()
        {
            customer_repo = new BaseAsyncRepository<Customer>();


            LoadedCommand = new DelegateCommand(LoadData);
            ShowInfoCommand = new DelegateCommand<int?>(ShowInfoCus);
            DeleteCusCommand = new DelegateCommand<int?>(DeleteCustomer);
            AddCusCommand = new DelegateCommand(ShowNewCus);
            RefreshCommand = new DelegateCommand(RefreshData);
            SubmitCommand = new DelegateCommand<string>(Submit);
            ChoseImages = new DelegateCommand(ChoseImg);
        }

        private async void ShowListOrder()
        {
            await DialogHost.Show(new CustomerOrder(), DialogHostId);
        }

        private void ChoseImg()
        {
            try
            {
                if (Op.ShowDialog() == true)
                {
                    ImgCus = new BitmapImage(new Uri(Op.FileName));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void Submit(string obj)
        {
            try
            {
                var newcus = new Customer()
                {
                    Email = Email,
                    Address = Address,
                    Avatar = ImgCus.ConvertToByte(),
                    Name = Name,
                    Phone = Phone,
                };


                if (IsB)
                    newcus.Sex = true;
                if (IsG)
                    newcus.Sex = false;

                if(Kind!=null)
                {
                    newcus.Kind = ListKinds.IndexOf(Kind);
                }

                if (string.IsNullOrEmpty(obj))
                {
                    //Create new customer

                    var objresult = await customer_repo.Add(newcus);

                    if (objresult != null)
                    {
                        dc = new DialogContent() { Content = "Thêm Thành Công", Tilte = "Thông Báo" };
                        dialog = new DialogOk() { DataContext = dc };
                        DialogHost.CloseDialogCommand.Execute(null, null);
                        await DialogHost.Show(dialog, DialogHostId);
                        ListCustomer.Add(objresult);
                    }
                    else
                    {
                        dc = new DialogContent() { Content = "Thêm Thất Bại", Tilte = "Thông Báo" };
                        dialog = new DialogOk() { DataContext = dc };
                        DialogHost.CloseDialogCommand.Execute(null, null);
                        await DialogHost.Show(dialog, DialogHostId);
                    }

                }
                else
                {
                    //update customer

                    newcus.CusID = Convert.ToInt32(obj);

                    if (await customer_repo.Update(newcus))
                    {
                        dc = new DialogContent() { Content = "Cập Nhật Thành Công", Tilte = "Thông Báo" };
                        dialog = new DialogOk() { DataContext = dc };
                        DialogHost.CloseDialogCommand.Execute(null, null);
                        await DialogHost.Show(dialog, DialogHostId);
                        ListCustomer = new ObservableCollection<Customer>(await customer_repo.GetAllAsync());
                    }
                    else
                    {
                        dc = new DialogContent() { Content = "Cập Nhật Thất Bại", Tilte = "Thông Báo" };
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

        private async void RefreshData()
        {
            ListCustomer = new ObservableCollection<Customer>(await customer_repo.GetAllAsync());
        }

        private async void ShowNewCus()
        {
            Name = CusID = Phone = Address = Email = null;
            ImgCus = null;
            IsB = IsG = false;
            Kind = null;
            await DialogHost.Show(new CustomerProfile(), DialogHostId);
        }

        private async void DeleteCustomer(int? obj)
        {
            try
            {
                dc = new DialogContent() { Content = "Bạn muốn xóa customer này ?", Tilte = "Thông Báo" };
                var dialogYS = new DialogYesNo() { DataContext = dc };
                var result = (bool)await DialogHost.Show(dialogYS, DialogHostId);
                if (result)
                {
                    if (obj != null)
                    {
                        if (await customer_repo.Remove((int)obj))
                        {
                            ListCustomer.Remove(ListCustomer.SingleOrDefault(t => t.CusID == (int)obj));
                            dc = new DialogContent() { Content = "Xóa Thành Công", Tilte = "Thông Báo" };
                            dialog = new DialogOk() { DataContext = dc };
                            await DialogHost.Show(dialog, DialogHostId);
                        }
                        else
                        {
                            dc = new DialogContent() { Content = "Xóa Thất Bại", Tilte = "Thông Báo" };
                            dialog = new DialogOk() { DataContext = dc };
                            await DialogHost.Show(dialog, DialogHostId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void ShowInfoCus(int? obj)
        {
            if(obj!=null)
            {
                var index = (int)obj;

                var cus = ListCustomer.SingleOrDefault(t => t.CusID == index);
                if(cus!=null)
                {
                    Name = cus.Name;
                    CusID = cus.CusID.ToString();
                    ImgCus = cus.Avatar.LoadImage();
                    Phone = cus.Phone;
                    if (cus.Sex == true)
                        IsB = true;
                    if (cus.Sex == false)
                        IsG = true;
                    Address = cus.Address;
                    Email = cus.Email;
                    if (cus.Kind != null)
                    {
                        int indexcus = (int)cus.Kind;
                        Kind = ListKinds.ElementAt(indexcus);
                    }
                    else Kind = null;
                }

                await DialogHost.Show(new CustomerProfile(), DialogHostId);

            }
        }

        private void LoadData()
        {
            List<Task> tasks = new List<Task>();

            tasks.Add(new Task(() => { ListCustomer = new ObservableCollection<Customer>(customer_repo.GetAll()); }));

            tasks.Add(new Task(() => { ListKey = new ObservableCollection<string>() { "CustomerID", "Customer Name" }; }));

            tasks.Add(new Task(() => { ListKinds = new ObservableCollection<string>() { "Thường", "Thân Mật", "Vip" }; }));

            tasks.ForEach(a => a.Start());


        }
    }
}
