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
    /// Логика взаимодействия для PageListServices.xaml
    /// </summary>
    public partial class PageListServices : Page
    {
        public List<Service> services;
        public PageListServices()
        {
            InitializeComponent();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var servicesForRemoving = DGridServices.SelectedItems.Cast<Service>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {servicesForRemoving.Count()} элементов?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    AtelierBaseEntities.GetContext().Service.RemoveRange(servicesForRemoving);
                    AtelierBaseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                    services = AtelierBaseEntities.GetContext().Service.ToList();
                    DGridServices.ItemsSource = services;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("С удаляемыми данными имеются связанные записи в других таблицах.", "Удаление отменено!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageService(null));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Manager.MainTextBlock.Text = "Каталог услуг";
            AtelierBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(o => o.Reload());
            services = AtelierBaseEntities.GetContext().Service.ToList();
            DGridServices.ItemsSource = services;
            tbSearch.Focus();
            Manager.BtnBack.Visibility = Visibility.Hidden;
            this.FontFamily = new FontFamily("Cambria");
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDGrid();
        }

        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDGrid();
        }

        private void BtnFormDiagram_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageDiagramServices());
        }

        private void UpdateDGrid()
        {
            if (cmbSort != null && tbSearch != null)
            {
                    services = AtelierBaseEntities.GetContext().Service.ToList();
                    foreach (var item in services)
                    {
                        int amount = AtelierBaseEntities.GetContext().Order.Count(o => o.IdService == item.Id);
                        item.NumberOfUses = amount;
                    }
                    if (tbSearch.Text.Length != 0)
                    {
                        services = services.Where(s => s.Title.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
                    }
                    if (cmbSort.SelectedIndex == 0)
                    {
                        services = services.OrderBy(O => O.Id).ToList();
                    }
                    else if (cmbSort.SelectedIndex == 1)
                    {
                        services = services.OrderBy(s => s.Cost).ToList();
                    }
                    else if (cmbSort.SelectedIndex == 2)
                    {
                        services = services.OrderByDescending(s => s.NumberOfUses).ToList();
                    }
                    DGridServices.ItemsSource = services;
                    if (DGridServices.Items.Count == 0)
                    {
                        DGridServices.Visibility = Visibility.Hidden;
                        tblNoResult.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        DGridServices.Visibility = Visibility.Visible;
                        tblNoResult.Visibility = Visibility.Hidden;
                    }
            }
        }
    }
}
