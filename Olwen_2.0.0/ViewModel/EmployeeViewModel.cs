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
    public class EmployeeViewModel:ViewModelBase
    {
        private IAsyncRepository<Employee> employee_Repo;
        private const string DialogHostId = "RootDialogHost2";
        private ObservableCollection<Employee> _listEmployee;
        private Employee _selectedEmp;
        private ObservableCollection<SalaryInfo> _listSalaryEmp;
        private string _dOB;
        private string _name, _address, _phone, _mission, _salary,_empID;
        private bool _isB, _isG;
        private ObservableCollection<string> _listKey;
        private string _sKey, _seaKey;
        static DialogContent dc = new DialogContent();
        static DialogOk dialog = new DialogOk();
        private BitmapImage _imgEmp;
        private static OpenFileDialog Op = new OpenFileDialog();

        public DelegateCommand LoadedCommand
        {
            get;
            private set;
        }

        public DelegateCommand<int?> DeleteEmpCommand
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

        public DelegateCommand<int?> ShowInfoCommand 
        {
            get;
            private set;
        }

        public ObservableCollection<Employee> ListEmployee
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

        public Employee SelectedEmp
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
                                ListEmployee = new ObservableCollection<Employee>(employee_Repo.GetFilter(t => t.EmpID == Convert.ToInt32(SeaKey)));
                            }
                        }
                        else
                        {
                            ListEmployee = new ObservableCollection<Employee>(employee_Repo.GetFilter(t => t.Name.Contains(SeaKey)));
                        }
                    }
                }
                else
                    ListEmployee = new ObservableCollection<Employee>(employee_Repo.GetAll());
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


        public EmployeeViewModel()
        {
            employee_Repo = new BaseAsyncRepository<Employee>();

            LoadedCommand = new DelegateCommand(CreateData);
            DeleteEmpCommand = new DelegateCommand<int?>(DeleteEmp);
            AddEmpCommand = new DelegateCommand(ShowNewEmp);
            ChoseImages = new DelegateCommand(ChoseImg);
            ShowInfoCommand = new DelegateCommand<int?>(ShowInfoEmp);
            SubmitCommand = new DelegateCommand<string>(Submit);
            RefreshCommand = new DelegateCommand(CreateData);
        }

        private async void Submit(string obj)
        {
            try
            {
                var newemp = new Employee()
                {
                    Mission = Mission,
                    Address = Address,
                    Avatar = ImgEmp.ConvertToByte(),
                    Name = Name,
                    Salary = Convert.ToDecimal(Salary),
                    Phone = Phone,
                    DOB = Convert.ToDateTime(DOB),
                };


                if (IsB)
                    newemp.Sex = true;
                if (IsG)
                    newemp.Sex = false;

                

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
                        ListEmployee.Add(objresult);
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
                        ListEmployee = new ObservableCollection<Employee>(await employee_Repo.GetAllAsync());
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

        private async void ShowInfoEmp(int? obj)
        {
            if (obj != null)
            {
                var index = (int)obj;

                var emp = ListEmployee.SingleOrDefault(t => t.EmpID == index);
                if (emp != null)
                {
                    Name = emp.Name;
                    EmpID = emp.EmpID.ToString();
                    ImgEmp = emp.Avatar.LoadImage();
                    Phone = emp.Phone;
                    IsB = IsG = false;
                    if (emp.Sex == true)
                        IsB = true;
                    if (emp.Sex == false)
                        IsG = true;
                    Address = emp.Address;
                    Mission = emp.Mission;
                    Salary = emp.Salary.ToString();
                    DOB = emp.DOB.ToString();
                }

                await DialogHost.Show(new EmployeeProfile(), DialogHostId);

            }
        }

        private async void ShowNewEmp()
        {
            EmpID = Name = DOB = Mission = Phone = Address = Salary = null;
            IsB = IsG = false;
            ImgEmp = null;
            await DialogHost.Show(new EmployeeProfile(), DialogHostId);
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
                dc = new DialogContent() { Content = "Bạn muốn xóa nhân viên này ?", Tilte = "Thông Báo" };
                var dialogYS = new DialogYesNo() { DataContext = dc };
                var result = (bool)await DialogHost.Show(dialogYS, DialogHostId);
                if (result)
                {
                    if (obj != null)
                    {
                        if (await employee_Repo.Remove((int)obj))
                        {
                            ListEmployee.Remove(ListEmployee.SingleOrDefault(t => t.EmpID == (int)obj));
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

            tasks.Add(new Task(() => { ListEmployee = new ObservableCollection<Employee>(employee_Repo.GetAll()); }));

            tasks.Add(new Task(() => { ListKey = new ObservableCollection<string>() { "EmployeeID", "Employee Name" }; }));

            tasks.ForEach(a => a.Start());
        }
    }
}
