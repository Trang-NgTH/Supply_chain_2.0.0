using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using Olwen_2._0._0.DependencyInjection;
using Olwen_2._0._0.Model;
using Olwen_2._0._0.Repositories.Implements;
using Olwen_2._0._0.Repositories.Interfaces;
using Olwen_2._0._0.View.DialogsResult;
using Olwen_2._0._0.View.Windows;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Olwen_2._0._0.ViewModel
{
    public class LoginViewModel:ViewModelBase
    {
        private IAsyncRepository<Account> acc_repo;
        static DialogContent dc = new DialogContent();
        static DialogOk dialog = new DialogOk();
        private const string DialogHostId = "RootDialogHost";
        private bool _isRememberLogin;

        private string _namelogin, _password;

        public ICommand PasswordChangedCommand { get; set; }

        public string Namelogin
        {
            get
            {
                return _namelogin;
            }

            set
            {
                _namelogin = value;
                RaisePropertyChanged();
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                RaisePropertyChanged();
            }
        }


        public DelegateCommand<Window> SignInCommand
        {
            get;
            private set;

        }

        public bool IsRememberLogin
        {
            get
            {
                return _isRememberLogin;
            }

            set
            {
                _isRememberLogin = value;
                RaisePropertyChanged();
            }
        }

        public LoginViewModel()
        {
            acc_repo = new BaseAsyncRepository<Account>();

            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { Password = p.Password; });

            SignInCommand = new DelegateCommand<Window>(SignIn);
        }

        private async void SignIn(Window obj)
        {
            try
            {
                var result = await acc_repo.GetFilterAsync(t => t.NameLogin.Equals(Namelogin) && t.PassWord.Equals(Password));
                if(result.Count()<1)
                {
                    dc.Content = "Sai Tài Khoản Hoặc Mật Khẩu";
                    dc.Tilte = "Thông Báo";
                    dialog = new DialogOk() { DataContext = dc };
                    DialogHost.CloseDialogCommand.Execute(null, null);
                    await DialogHost.Show(dialog, DialogHostId);
                }
                else
                {
                    if (IsRememberLogin)
                    {
                        StateLogin.AccountLogin.NameLogin = Namelogin;
                        StateLogin.AccountLogin.Password = Password;
                        StateLogin.AccountLogin.Id = result.First().Employee.EmpID;
                        StateLogin.AccountLogin.Username = result.First().Employee.Name;
                        StateLogin.AccountLogin.Image = result.First().Avatar;
                        StateLogin.WrireJson();
                    }

                    dc.Content = "Đăng Nhập Thành Công";
                    dc.Tilte = "Thông Báo";
                    dialog = new DialogOk() { DataContext = dc };
                    DialogHost.CloseDialogCommand.Execute(null, null);
                    await DialogHost.Show(dialog, DialogHostId);
                    
                    obj.Hide();
                    var view = new MainWindow();
                    view.Show();
                    obj.Close();
                }
            
                
            }
            catch(Exception ex)
            {
                //Raiserror();
                MessageBox.Show(ex.ToString());
            }
        }

        private async void Raiserror()
        {
            dc.Content = "Có Lỗi";
            dc.Tilte = "Thông Báo";
            dialog = new DialogOk() { DataContext = dc };
            DialogHost.CloseDialogCommand.Execute(null, null);
            await DialogHost.Show(dialog, DialogHostId);
        }
    }
}
