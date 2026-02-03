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
    /// Логика взаимодействия для PageListTextile.xaml
    /// </summary>
    public partial class PageListTextile : Page
    {
        public List<Textile> textile;
        public PageListTextile()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageTextile(null));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var textileForRemoving = DGridTextile.SelectedItems.Cast<Textile>().ToList();
            int i = 0;
            if (MessageBox.Show($"Вы точно хотите удалить следующие {textileForRemoving.Count()} элементов?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                foreach (Textile item in textileForRemoving)
                {
                    if (AtelierBaseEntities.GetContext().Order.Where(o => o.IdTextile == item.Id).ToList().FirstOrDefault() != null)
                    {
                        MessageBox.Show("С удаляемыми данными имеются связанные записи в других таблицах.", "Удаление отменено!", MessageBoxButton.OK, MessageBoxImage.Error);
                        i = 1;
                        break;
                    }
                }
                if (i == 0)
                {
                    try
                    {
                        AtelierBaseEntities.GetContext().Textile.RemoveRange(textileForRemoving);
                        AtelierBaseEntities.GetContext().SaveChanges();
                        MessageBox.Show("Данные удалены!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                        textile = AtelierBaseEntities.GetContext().Textile.ToList();
                        DGridTextile.ItemsSource = textile;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("При удалении данных возникли неполадки!", "Удаление отменено!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Services.Manager.MainTextBlock.Text = "Каталог тканей";
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

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void rbAll_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void rbInStock_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid();
        }
        private void UpdateDataGrid()
        {
            if (tbSearch != null && rbAll != null)
            {
                if (rbAll.IsChecked == true)
                {
                    textile = AtelierBaseEntities.GetContext().Textile.ToList();
                }
                else if (rbInStock.IsChecked == true)
                {
                    textile = AtelierBaseEntities.GetContext().Textile.Where(t => t.InStock == true).ToList();
                }
                if (tbSearch.Text.Length != 0)
                {
                    textile = textile.Where(t => t.Title.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
                }
                switch (cmbSort.SelectedIndex)
                {
                    case 0:
                        textile = textile.OrderBy(t => t.Id).ToList();
                        break;
                    case 1:
                        textile = textile.OrderBy(t => t.Cost).ToList();
                        break;
                    case 2:
                        textile = textile.OrderByDescending(t => t.Cost).ToList();
                        break;
                }
                DGridTextile.ItemsSource = textile;
                if (DGridTextile.Items.Count == 0)
                {
                    tblSearch.Visibility = Visibility.Visible;
                    DGridTextile.Visibility = Visibility.Hidden;
                }
                else
                {
                    tblSearch.Visibility = Visibility.Hidden;
                    DGridTextile.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
