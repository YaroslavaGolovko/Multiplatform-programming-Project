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
    /// Логика взаимодействия для PageListOrders.xaml
    /// </summary>
    public partial class PageListOrders : Page
    {
        public List<Order> orders;
        public PageListOrders()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Services.Manager.MainTextBlock.Text = "Заказы";
            AtelierBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(o => o.Reload());
            UpdateDataGrid();
            tbSearch.Focus();
            this.FontFamily = new FontFamily("Cambria");
            Manager.BtnBack.Visibility = Visibility.Hidden;
        }
        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var ordersForRemoving = DGridOrders.SelectedItems.Cast<Order>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {ordersForRemoving.Count()} элементов?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    AtelierBaseEntities.GetContext().Order.RemoveRange(ordersForRemoving);
                    AtelierBaseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                    orders = AtelierBaseEntities.GetContext().Order.ToList();
                    DGridOrders.ItemsSource = orders;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("С удаляемыми данными имеются связанные записи в других таблицах.", "Удаление отменено!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageOrder(null));
        }

        private void btnCreateReport_Click(object sender, RoutedEventArgs e)
        {
            Services.Manager.ShowMessage();
        }

        private void UpdateDataGrid()
        {
            if (cmbSort != null && cmbFilter != null && tbSearch != null)
            {
                if (cmbFilter.SelectedIndex == 0)
                {
                    orders = AtelierBaseEntities.GetContext().Order.ToList();
                    if (tbSearch.Text.Length != 0)
                    {
                        orders = orders.Where(o => o.Service.Title.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
                    }
                }
                else if (cmbFilter.SelectedIndex == 1)
                {
                    orders = AtelierBaseEntities.GetContext().Order.Where(o => o.IsCompleted == true).ToList();
                    if (tbSearch.Text.Length != 0)
                    {
                        orders = orders.Where(o => o.Service.Title.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
                    }
                }
                else if (cmbFilter.SelectedIndex == 2)
                {
                    orders = AtelierBaseEntities.GetContext().Order.Where(o => o.IsCompleted == false).ToList();
                    if (tbSearch.Text.Length != 0)
                    {
                        orders = orders.Where(o => o.Service.Title.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
                    }
                }
                if (cmbSort.SelectedIndex == 0)
                {
                    orders = orders.OrderBy(O => O.Id).ToList();
                }
                else if (cmbSort.SelectedIndex == 1)
                {
                    orders = orders.OrderBy(O => O.RegistrationDate).ToList();
                }
                else if (cmbSort.SelectedIndex == 2)
                {
                    orders = orders.OrderBy(O => O.Price).ToList();
                }
                DGridOrders.ItemsSource = orders;
                if (DGridOrders.Items.Count == 0)
                {
                    DGridOrders.Visibility = Visibility.Hidden;
                    tblNoResult.Visibility = Visibility.Visible;
                }
                else
                {
                    DGridOrders.Visibility = Visibility.Visible;
                    tblNoResult.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
