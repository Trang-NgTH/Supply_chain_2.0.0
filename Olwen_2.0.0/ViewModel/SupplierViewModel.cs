using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
    public class SupplierViewModel:ViewModelBase
    {
        private IAsyncRepository<Supplier> supplier_repo;
        private const string DialogHostId = "RootDialogHost3";
        private ObservableCollection<Supplier> _listSupplier;
        private ObservableCollection<string> _listKey;
        private string _sKey, _seaKey;
        static DialogContent dc = new DialogContent();
        static DialogOk dialog = new DialogOk();
        private OpenFileDialog Op = new OpenFileDialog();
        private string _supID, _name, _phone, _email, _delegate;
        private BitmapImage _supImg;

        public DelegateCommand LoadedCommand
        {
            get;
            private set;
        }

        public DelegateCommand<int?> ShowInfoCommand
        {
            get;
            private set;
        }

        public DelegateCommand<int?> DeleteSupCommand
        {
            get;
            private set;
        }

        public DelegateCommand AddSupCommand
        {
            get;
            private set;
        }

        public DelegateCommand ChoseImages
        {
            get;
            private set;
        }

        public DelegateCommand<string> SubmitCommand
        {
            get;
            private set;
        }

        public DelegateCommand RefreshCommand
        {
            get;
            private set;
        }

        public ObservableCollection<Supplier> ListSupplier
        {
            get
            {
                return _listSupplier;
            }

            set
            {
                _listSupplier = value;
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
                if (!string.IsNullOrEmpty(_seaKey))
                {
                    if (!string.IsNullOrEmpty(SKey))
                    {
                        if (ListKey.IndexOf(SKey) == 0)
                        {
                            if (SeaKey.IsNum())
                            {
                                ListSupplier = new ObservableCollection<Supplier>(supplier_repo.GetFilter(t => t.SupID == Convert.ToInt32(SeaKey)));
                            }
                        }
                        else
                        {
                            ListSupplier = new ObservableCollection<Supplier>(supplier_repo.GetFilter(t => t.Name.Contains(SeaKey)));
                        }
                    }
                }
                else
                    ListSupplier = new ObservableCollection<Supplier>(supplier_repo.GetAll());
            }
        }

        public string SupID
        {
            get
            {
                return _supID;
            }

            set
            {
                _supID = value;
                RaisePropertyChanged();
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

        public string Delegate
        {
            get
            {
                return _delegate;
            }

            set
            {
                _delegate = value;
                RaisePropertyChanged();
            }
        }

        public BitmapImage SupImg
        {
            get
            {
                return _supImg;
            }

            set
            {
                _supImg = value;
                RaisePropertyChanged();
            }
        }


        public SupplierViewModel()
        {
            supplier_repo = new BaseAsyncRepository<Supplier>();

            LoadedCommand = new DelegateCommand(CreateData);
            ShowInfoCommand = new DelegateCommand<int?>(ShowInfo);
            DeleteSupCommand = new DelegateCommand<int?>(DeleteSup);
            AddSupCommand = new DelegateCommand(ShowNewSup);
            ChoseImages = new DelegateCommand(ShowImg);
            SubmitCommand = new DelegateCommand<string>(Submit);
            RefreshCommand = new DelegateCommand(CreateData);
        }

        private async void Submit(string obj)
        {
            try
            {
                var newsup = new Supplier()
                {
                    Name = Name,
                    Delegate = Delegate,
                    Email = Email,
                    Phone = Phone,
                };

                if (SupImg != null)
                    newsup.Avatar = SupImg.ConvertToByte();


                if (obj==null)
                {
                    //Create new sup
                    var objresult = await supplier_repo.Add(newsup);

                    if (objresult != null)
                    {
                        dc = new DialogContent() { Content = "Thêm Thành Công", Tilte = "Thông Báo" };
                        dialog = new DialogOk() { DataContext = dc };
                        DialogHost.CloseDialogCommand.Execute(null, null);
                        await DialogHost.Show(dialog, DialogHostId);
                        ListSupplier.Add(objresult);
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
                    //update sup

                    newsup.SupID = Convert.ToInt32(obj);

                    if (await supplier_repo.Update(newsup))
                    {
                        dc = new DialogContent() { Content = "Cập Nhật Thành Công", Tilte = "Thông Báo" };
                        dialog = new DialogOk() { DataContext = dc };
                        DialogHost.CloseDialogCommand.Execute(null, null);
                        await DialogHost.Show(dialog, DialogHostId);
                        ListSupplier = new ObservableCollection<Supplier>(await supplier_repo.GetAllAsync());
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

        private async void ShowImg()
        {
            try
            {
                if (Op.ShowDialog() == true)
                {
                    SupImg = new BitmapImage(new Uri(Op.FileName));
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

        private async void ShowNewSup()
        {
            try
            {
                Name = Delegate = Phone = Email = null;
                SupID = null;
                SupImg = null;
                DialogHost.CloseDialogCommand.Execute(null, null);
                await DialogHost.Show(new SupplierProfile(), DialogHostId);

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

        private async void DeleteSup(int? obj)
        {
            try
            {
                dc = new DialogContent() { Content = "Bạn muốn xóa nhà cung cấp này ?", Tilte = "Thông Báo" };
                var dialogYS = new DialogYesNo() { DataContext = dc };
                var result = (bool)await DialogHost.Show(dialogYS, DialogHostId);
                if (result)
                {
                    if (obj != null)
                    {
                        if (await supplier_repo.Remove((int)obj))
                        {
                            ListSupplier.Remove(ListSupplier.SingleOrDefault(t => t.SupID == (int)obj));
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
                dc.Content = "Có Lỗi";
                dc.Tilte = "Thông Báo";
                dialog = new DialogOk() { DataContext = dc };
                DialogHost.CloseDialogCommand.Execute(null, null);
                await DialogHost.Show(dialog, DialogHostId);
            }
        }

        private async void ShowInfo(int? obj)
        {
            try
            {
                if (obj != null)
                {
                    var index = (int)obj;
                    var objsup = ListSupplier.SingleOrDefault(t => t.SupID == index);
                    Name = objsup.Name;
                    Delegate = objsup.Delegate;
                    Phone = objsup.Phone;
                    Email = objsup.Email;
                    SupID = obj.ToString();
                    SupImg = objsup.Avatar.LoadImage();
                    DialogHost.CloseDialogCommand.Execute(null, null);
                    await DialogHost.Show(new SupplierProfile(), DialogHostId);
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

        private void CreateData()
        {
            List<Task> tasks = new List<Task>();

            tasks.Add(new Task(() => { ListSupplier = new ObservableCollection<Supplier>(supplier_repo.GetAll()); }));

            tasks.Add(new Task(() => { ListKey = new ObservableCollection<string>() { "SupplierID", "Supllier Name" }; }));

            tasks.ForEach(a => a.Start());
        }
    }
}
