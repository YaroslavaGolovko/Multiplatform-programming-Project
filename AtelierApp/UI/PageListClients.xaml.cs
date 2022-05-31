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
    /// Логика взаимодействия для PageListClients.xaml
    /// </summary>
    public partial class PageListClients : Page
    {
        private List<Client> clients;
        public PageListClients()
        {
            InitializeComponent();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var clientsForRemoving = DGridClients.SelectedItems.Cast<Client>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {clientsForRemoving.Count()} элементов?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    AtelierBaseEntities.GetContext().Client.RemoveRange(clientsForRemoving);
                    AtelierBaseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                    clients = AtelierBaseEntities.GetContext().Client.ToList();
                    DGridClients.ItemsSource = clients;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("С удаляемыми данными имеются связанные записи в других таблицах.", "Удаление отменено!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageClient(null));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Manager.MainTextBlock.Text = "Клиенты";
            AtelierBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(o => o.Reload());
            clients = AtelierBaseEntities.GetContext().Client.ToList();
            DGridClients.ItemsSource = clients;
            tbSearch.Focus();
            Manager.BtnBack.Visibility = Visibility.Hidden;
            this.FontFamily = new FontFamily("Cambria");
        }

        private void rbAll_Click(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        private void rbFemale_Click(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        private void rbMale_Click(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            if (cmbSort != null && tbSearch != null)
            {
                if (rbAll.IsChecked == true)
                {
                    clients = AtelierBaseEntities.GetContext().Client.ToList();
                }
                else if (rbFemale.IsChecked == true)
                {
                    clients = AtelierBaseEntities.GetContext().Client.Where(c => c.IdGender == "ж").ToList();
                }
                else if (rbMale.IsChecked == true)
                {
                    clients = AtelierBaseEntities.GetContext().Client.Where(c => c.IdGender == "м").ToList();
                }
                if (tbSearch.Text.Length != 0)
                {
                    clients = clients.Where(c => c.LastName.ToLower().Contains(tbSearch.Text.ToLower()) || c.FirstName.ToLower().Contains(tbSearch.Text.ToLower())
                    || c.Patronymic.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
                }
                if (cmbSort.SelectedIndex == 0)
                {
                    clients = clients.OrderBy(O => O.Id).ToList();
                }
                else if (cmbSort.SelectedIndex == 1)
                {
                    clients = clients.OrderBy(c => c.LastName).ToList();
                }
                DGridClients.ItemsSource = clients;
                if (DGridClients.Items.Count == 0)
                {
                    DGridClients.Visibility = Visibility.Hidden;
                    tblNoResult.Visibility = Visibility.Visible;
                }
                else
                {
                    DGridClients.Visibility = Visibility.Visible;
                    tblNoResult.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
