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
    /// Логика взаимодействия для PageClient.xaml
    /// </summary>
    public partial class PageClient : Page
    {
        private Client _currentClient = new Client();
        public PageClient(Client selectedClient)
        {
            InitializeComponent();
            if (selectedClient != null)
            {
                _currentClient = selectedClient;
            }
            else
            {
                _currentClient.RegistrationDate = DateTime.Today;
            }
            DataContext = _currentClient;
            this.FontFamily = new FontFamily("Cambria");
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (tbLastName.Text.Length == 0)
                errors.AppendLine("Необходимо указать фамилию клиента");
            if (tbFirstName.Text.Length == 0)
                errors.AppendLine("Необходимо указать имя клиента");
            if (tbPatronymic.Text.Length == 0)
                errors.AppendLine("Необходимо указать отчество клиента");
            if (tbPhone.Text.Length == 0)
                errors.AppendLine("Необходимо указать контактный номер клиента");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (rbFemale.IsChecked == true)
            {
                _currentClient.IdGender = "ж";
            }
            if (rbMale.IsChecked == true)
            {
                _currentClient.IdGender = "м";
            }

            if (_currentClient.Id == 0)
            {
                AtelierBaseEntities.GetContext().Client.Add(_currentClient);
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
                AtelierBaseEntities.GetContext().Client.Remove(_currentClient);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_currentClient.Id != 0)
                Manager.MainTextBlock.Text = "Редактирование данных клиента";
            else
                Manager.MainTextBlock.Text = "Новый клиент";
            if (_currentClient.IdGender == "м")
                rbMale.IsChecked = true;
            Manager.BtnBack.Visibility = Visibility.Visible;
        }
    }
}
