using AtelierApp.Data;
using AtelierApp.UI;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AtelierApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class WindowStart : Window
    {
        public WindowStart()
        {
            InitializeComponent();
            this.FontFamily = new FontFamily("Cambria");
            Services.Manager.MainFrame = MainFrame;
            Services.Manager.MainTextBlock = MainTextBlock;
            tbLogin.Focus();
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            Services.Authorization.userLogin = tbLogin.Text;
            IEnumerable<string> logins = AtelierBaseEntities.GetContext().Worker.Select(user => user.Login).ToList();
            IEnumerable<string> passwords = AtelierBaseEntities.GetContext().Worker.Where(user => user.Login == tbLogin.Text).Select(user => user.Password).ToList();
            if (pbPassword.Visibility == Visibility.Visible)
            {
                if (Services.Authorization.CheckEnter(tbLogin.Text, logins, pbPassword.Password, passwords))
                {
                    WindowWork windowWork = new WindowWork();
                    windowWork.Show();
                    this.Close();
                }
            }
            else if (tbPassword.Visibility == Visibility.Visible)
                if (Services.Authorization.CheckEnter(tbLogin.Text, logins, tbPassword.Text, passwords))
                {
                    WindowWork windowWork = new WindowWork();
                    windowWork.Show();
                    this.Close();
                }
            tbLogin.Clear();
            pbPassword.Clear();
            tbPassword.Clear();
        }

        private void CbPassword_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.IsChecked.Value)
            {
                tbPassword.Text = pbPassword.Password;
                tbPassword.Visibility = Visibility.Visible;
                pbPassword.Visibility = Visibility.Hidden;
            }
            else
            {
                pbPassword.Password = tbPassword.Text;
                pbPassword.Visibility = Visibility.Visible;
                tbPassword.Visibility = Visibility.Hidden;
            }
        }
    }
}
