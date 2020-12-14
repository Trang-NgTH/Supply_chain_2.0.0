using Olwen_2._0._0.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Olwen_2._0._0.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;

        }

        private void Chip_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Bạn Muốn Đăng Xuất ?",
                                "Thông Báo",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Warning)
                                == MessageBoxResult.Yes)
            {
                StateLogin.AccountLogin = null;
                StateLogin.WrireJson();
                LoginWindow view = new LoginWindow();
                this.Hide();
                view.Show();
                this.Close();
            }
        }
    }

    
}
