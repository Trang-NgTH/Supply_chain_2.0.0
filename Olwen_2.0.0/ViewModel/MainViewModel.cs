using System;
using System.Windows;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using Olwen_2._0._0.DependencyInjection;
using Olwen_2._0._0.View.Windows;
using Prism.Commands;

namespace Olwen_2._0._0.ViewModel
{
  
    public class MainViewModel : ViewModelBase
    {
        private int _slide;
        private string _userName;
        private BitmapImage _image;

        public DelegateCommand<int?> SlideChange
        {
            get;
            private set;
        }

        public int Slide
        {
            get
            {
                return _slide;
            }

            set
            {
                _slide = value;
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

        public BitmapImage Image
        {
            get
            {
                return _image;
            }

            set
            {
                _image = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand<Window> LogoutCommand
        {
            get;
            set;
        }

        public MainViewModel()
        {
            Slide = 0;
            UserName = StateLogin.AccountLogin.Username;
            if (StateLogin.AccountLogin.Image != null)
                Image = StateLogin.AccountLogin.Image.LoadImage();

            SlideChange = new DelegateCommand<int?>(ChangeSlide);
            LogoutCommand = new DelegateCommand<Window>(Logout);

        }

        private void Logout(Window obj)
        {
            StateLogin.AccountLogin = null;
            StateLogin.WrireJson();
            LoginWindow view = new LoginWindow();
            obj.Hide();
            view.Show();
            obj.Close();
        }

        private void ChangeSlide(int? obj)
        {
            try
            {
                if(obj!=null)
                {
                    Slide = (int)obj;
                }
            }
            catch
            {
                MessageBox.Show("có lỗi");
            }
        }
    }
}