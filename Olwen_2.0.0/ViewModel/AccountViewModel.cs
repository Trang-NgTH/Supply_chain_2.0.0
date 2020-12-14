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
    public class AccountViewModel:ViewModelBase
    {
        private IAsyncRepository<Account> employee_Repo;
        private const string DialogHostId = "RootDialogHost212";
        private ObservableCollection<Account> _listEmployee;
        private Account _selectedEmp;
        private ObservableCollection<SalaryInfo> _listSalaryEmp;
        private string _dOB;
        private string _userName,_nameLogin, _address, _phone, _mission, _salary, _empID;
        private bool _isB, _isG;
        private ObservableCollection<string> _listKey;
        private string _sKey, _seaKey;
        static DialogContent dc = new DialogContent();
        static DialogOk dialog = new DialogOk();
        private BitmapImage _imgEmp;
        private static OpenFileDialog Op = new OpenFileDialog();

        private bool _isE;

        public DelegateCommand LoadedCommand
        {
            get;
            private set;
        }

        public DelegateCommand<string> DeleteEmpCommand
        {
            get;
            private set;
        }

        public DelegateCommand AddEmpCommand
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

        public DelegateCommand<string> ShowInfoCommand
        {
            get;
            private set;
        }

        public ObservableCollection<Account> ListAccount
        {
            get
            {
                return _listEmployee;
            }

            set
            {
                _listEmployee = value;
                RaisePropertyChanged();
            }
        }

        public Account SelectedEmp
        {
            get
            {
                return _selectedEmp;
            }

            set
            {
                _selectedEmp = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<SalaryInfo> ListSalaryEmp
        {
            get
            {
                return _listSalaryEmp;
            }

            set
            {
                _listSalaryEmp = value;
                RaisePropertyChanged();
            }
        }

        public string DOB
        {
            get
            {
                return _dOB;
            }

            set
            {
                _dOB = value;
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

        public string Mission
        {
            get
            {
                return _mission;
            }

            set
            {
                _mission = value;
                RaisePropertyChanged();
            }
        }

        public string Salary
        {
            get
            {
                return _salary;
            }

            set
            {
                _salary = value;
                RaisePropertyChanged();
            }
        }

        public bool IsB
        {
            get
            {
                return _isB;
            }

            set
            {
                _isB = value;
                RaisePropertyChanged("IsB");
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
                RaisePropertyChanged("IsG");
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
                RaisePropertyChanged();
                if (!string.IsNullOrEmpty(_seaKey))
                {
                    if (!string.IsNullOrEmpty(SKey))
                    {
                        if (ListKey.IndexOf(SKey) == 0)
                        {
                            if (SeaKey.IsNum())
                            {
                               // ListAccount = new ObservableCollection<Account>(employee_Repo.GetFilter(t => t.EmpID == Convert.ToInt32(SeaKey)));
                            }
                        }
                        else
                        {
                            ListAccount = new ObservableCollection<Account>(employee_Repo.GetFilter(t => t.UserName.Contains(SeaKey)));
                        }
                    }
                }
                else
                    ListAccount = new ObservableCollection<Account>(employee_Repo.GetAll());
            }
        }

        public BitmapImage ImgEmp
        {
            get
            {
                return _imgEmp;
            }

            set
            {
                _imgEmp = value;
                RaisePropertyChanged();
            }
        }

        public string EmpID
        {
            get
            {
                return _empID;
            }

            set
            {
                _empID = value;
                RaisePropertyChanged();
            }
        }

        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                _userName = value;
                RaisePropertyChanged();
            }
        }

        public string NameLogin
        {
            get
            {
                return _nameLogin;
            }

            set
            {
                _nameLogin = value;
                RaisePropertyChanged();
            }
        }

        public bool IsE
        {
            get
            {
                return _isE;
            }

            set
            {
                _isE = value;
                RaisePropertyChanged();
            }
        }

        public AccountViewModel()
        {
            employee_Repo = new BaseAsyncRepository<Account>();

            LoadedCommand = new DelegateCommand(CreateData);
           // DeleteEmpCommand = new DelegateCommand<string>(DeleteEmp);
            AddEmpCommand = new DelegateCommand(ShowNewEmp);
            ChoseImages = new DelegateCommand(ChoseImg);
            ShowInfoCommand = new DelegateCommand<string>(ShowInfoEmp);
            SubmitCommand = new DelegateCommand<string>(Submit);
            RefreshCommand = new DelegateCommand(CreateData);
        }

        private async void Submit(string obj)
        {
            try
            {
                var newemp = new Account()
                {
                    Address = Address,
                    Avatar = ImgEmp.ConvertToByte(),
                    UserName = UserName,
                    NameLogin = NameLogin,
                    Phone = Phone,
                    DOB = Convert.ToDateTime(DOB),
                };


                if (IsB)
                    newemp.Sex = true;
                if (IsG)
                    newemp.Sex = false;

                if(IsE==true)
                {
                    newemp.Role = 1;
                }

                if (string.IsNullOrEmpty(obj))
                {
                    //Create new customer

                    var objresult = await employee_Repo.Add(newemp);

                    if (objresult != null)
                    {
                        dc = new DialogContent() { Content = "Thêm Thành Công", Tilte = "Thông Báo" };
                        dialog = new DialogOk() { DataContext = dc };
                        DialogHost.CloseDialogCommand.Execute(null, null);
                        await DialogHost.Show(dialog, DialogHostId);
                        ListAccount.Add(objresult);
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

                    newemp.EmpID = Convert.ToInt32(obj);

                    if (await employee_Repo.Update(newemp))
                    {
                        dc = new DialogContent() { Content = "Cập Nhật Thành Công", Tilte = "Thông Báo" };
                        dialog = new DialogOk() { DataContext = dc };
                        DialogHost.CloseDialogCommand.Execute(null, null);
                        await DialogHost.Show(dialog, DialogHostId);
                        ListAccount = new ObservableCollection<Account>(await employee_Repo.GetAllAsync());
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

        private async void ShowInfoEmp(string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                var emp = ListAccount.SingleOrDefault(t => t.NameLogin.Equals(obj));
                if (emp != null)
                {
                    UserName = emp.UserName;
                    NameLogin = emp.NameLogin;
                    EmpID = emp.EmpID.ToString();
                    ImgEmp = emp.Avatar.LoadImage();
                    Phone = emp.Phone;
                    IsB = IsG = false;
                    if (emp.Sex == true)
                        IsB = true;
                    if (emp.Sex == false)
                        IsG = true;
                    if (emp.Role == 0)
                        IsE = true;
                    if (emp.Role == -1)
                        IsE = false;
                    Address = emp.Address;
                    DOB = emp.DOB.ToString();
                }
                DialogHost.CloseDialogCommand.Execute(null, null);
                await DialogHost.Show(new AccountProfile(), DialogHostId);

            }
        }

        private async void ShowNewEmp()
        {
            EmpID = UserName = NameLogin = DOB = Mission = Phone = Address = Salary = null;
            IsB = IsG = false;
            IsE = false;
            ImgEmp = null;
            await DialogHost.Show(new AccountProfile(), DialogHostId);
        }

        private void ChoseImg()
        {
            try
            {
                if (Op.ShowDialog() == true)
                {
                    ImgEmp = new BitmapImage(new Uri(Op.FileName));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void DeleteEmp(int? obj)
        {
            try
            {
                dc = new DialogContent() { Content = "Bạn muốn xóa tài khoản này ?", Tilte = "Thông Báo" };
                var dialogYS = new DialogYesNo() { DataContext = dc };
                var result = (bool)await DialogHost.Show(dialogYS, DialogHostId);
                if (result)
                {
                    if (obj != null)
                    {
                        if (await employee_Repo.Remove((int)obj))
                        {
                            ListAccount.Remove(ListAccount.SingleOrDefault(t => t.EmpID == (int)obj));
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

        private void CreateData()
        {
            List<Task> tasks = new List<Task>();

            tasks.Add(new Task(() => { ListAccount = new ObservableCollection<Account>(employee_Repo.GetAll()); }));

            tasks.Add(new Task(() => { ListKey = new ObservableCollection<string>() { "AccountID", "Account Name" }; }));

            tasks.ForEach(a => a.Start());
        }
    }
}
