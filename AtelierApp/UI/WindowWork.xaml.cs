using System;
using System.Collections.Generic;
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

namespace AtelierApp.UI
{
    /// <summary>
    /// Логика взаимодействия для WindowWork.xaml
    /// </summary>
    public partial class WindowWork : Window
    {
        public WindowWork()
        {
            InitializeComponent();
            this.FontFamily = new FontFamily("Cambria");
            Services.Manager.MainFrame = MainFrame;
            Services.Manager.MainTextBlock = MainTextBlock;
            Services.Manager.BtnBack = btnBack;
            Services.Manager.BtnBack.Visibility = Visibility.Hidden;
            Services.Manager.MainFrame.Navigate(new PageMenu());
        }
        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            Services.Manager.MainFrame.Navigate(new PageListOrders());
        }

        private void BtnClient_Click(object sender, RoutedEventArgs e)
        {
            Services.Manager.MainFrame.Navigate(new PageListClients());
        }

        private void BtnWorker_Click(object sender, RoutedEventArgs e)
        {
            Services.Manager.MainFrame.Navigate(new PageListWorkers());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Services.Manager.MainTextBlock.Text = "Меню администратора";
            tblUser.Text = "Логин: " + Services.Authorization.userLogin;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Services.Manager.MainFrame.GoBack();
        }

        private void btnTextile_Click(object sender, RoutedEventArgs e)
        {
            Services.Manager.MainFrame.Navigate(new PageListTextile());
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            WindowStart window = new WindowStart();
            window.Show();
            this.Close();
        }

        private void btnService_Click(object sender, RoutedEventArgs e)
        {
            Services.Manager.MainFrame.Navigate(new PageListServices());
        }
    }
}
