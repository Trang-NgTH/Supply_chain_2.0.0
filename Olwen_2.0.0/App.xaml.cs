using Olwen_2._0._0.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Olwen_2._0._0
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            StateLogin.ReadJson();

            if (!string.IsNullOrEmpty(StateLogin.AccountLogin.NameLogin))
            {
                StartupUri = new Uri("View/Windows/MainWindow.xaml", UriKind.Relative);
            }
            else
                StartupUri = new Uri("View/Windows/LoginWindow.xaml", UriKind.Relative);
        }
    }
}
