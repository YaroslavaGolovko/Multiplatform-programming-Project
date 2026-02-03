using AtelierApp.Data;
using Services;
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

namespace AtelierApp.UI
{
    /// <summary>
    /// Логика взаимодействия для PageWorker.xaml
    /// </summary>
    public partial class PageWorker : Page
    {
        private Worker _currentWorker = new Worker();
        public PageWorker(Worker selectedWorker)
        {
            InitializeComponent();
            if (selectedWorker != null)
            {
                _currentWorker = selectedWorker;
            }
            DataContext = _currentWorker;
            this.FontFamily = new FontFamily("Cambria");
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (tbLastName.Text.Length == 0)
                errors.AppendLine("Необходимо указать фамилию сотрудника");
            if (tbFirstName.Text.Length == 0)
                errors.AppendLine("Необходимо указать имя сотрудника");
            if (tbPatronymic.Text.Length == 0)
                errors.AppendLine("Необходимо указать отчество сотрудника");
            if (rbAdmin.IsChecked == true)
            {
                if(tbLogin.Text.Length==0)
                    errors.AppendLine("Необходимо указать логин для администратора");
                if (tbPassword.Text.Length == 0)
                    errors.AppendLine("Необходимо указать пароль для администратора");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (rbTailor.IsChecked == true)
            {
                _currentWorker.IdType = 2;
            }
            if (_currentWorker.Id == 0)
            {
                if (_currentWorker.IdType == 3)
                {
                    if(!Authorization.CheckPassword(tbPassword.Text))
                        MessageBox.Show("Введенный пароль не соотвествует требованиям!", "Требуется изменить пароль", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                AtelierBaseEntities.GetContext().Worker.Add(_currentWorker);
            }

            try
            {
                AtelierBaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                AtelierBaseEntities.GetContext().Worker.Remove(_currentWorker);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_currentWorker.Id != 0)
            {
                Manager.MainTextBlock.Text = "Редактирование данных клиента";
                if (_currentWorker.TypeOfWorker.Name == "Директор")
                    rbDirector.IsChecked = true;
                if (_currentWorker.TypeOfWorker.Name == "Администратор")
                    rbAdmin.IsChecked = true;
                if (_currentWorker.TypeOfWorker.Name == "Портной")
                    rbTailor.IsChecked = true;
            }
            else
                Manager.MainTextBlock.Text = "Новый клиент";
            if (rbAdmin.IsChecked == true)
            {
                tbLogin.Visibility = Visibility.Visible;
                tbPassword.Visibility = Visibility.Visible;
                tblLogin.Visibility = Visibility.Visible;
                tblPassword.Visibility = Visibility.Visible;
            }
            else
            {
                tbLogin.Visibility = Visibility.Hidden;
                tbPassword.Visibility = Visibility.Hidden;
                tblLogin.Visibility = Visibility.Hidden;
                tblPassword.Visibility = Visibility.Hidden;
            }
            Manager.BtnBack.Visibility = Visibility.Visible;
            tbFirstName.Focus();
        }

        private void rbDirector_Click(object sender, RoutedEventArgs e)
        {
            _currentWorker.IdType = 1;
            tbLogin.Visibility = Visibility.Hidden;
            tbPassword.Visibility = Visibility.Hidden;
            tblLogin.Visibility = Visibility.Hidden;
            tblPassword.Visibility = Visibility.Hidden;
        }

        private void rbAdmin_Click(object sender, RoutedEventArgs e)
        {
            _currentWorker.IdType = 3;
            tbLogin.Visibility = Visibility.Visible;
            tbPassword.Visibility = Visibility.Visible;
            tblLogin.Visibility = Visibility.Visible;
            tblPassword.Visibility = Visibility.Visible;
        }

        private void rbTailor_Click(object sender, RoutedEventArgs e)
        {
            _currentWorker.IdType = 2;
            tbLogin.Visibility = Visibility.Hidden;
            tbPassword.Visibility = Visibility.Hidden;
            tblLogin.Visibility = Visibility.Hidden;
            tblPassword.Visibility = Visibility.Hidden;
        }
    }
}
