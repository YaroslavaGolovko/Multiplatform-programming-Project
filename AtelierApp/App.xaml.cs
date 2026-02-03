using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AtelierApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Во время работы приложения возникли неполадки! Попробуйте войти позже.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            e.Handled = true;
            Environment.Exit(0);
        }
    }
}
