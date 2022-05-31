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
    /// Логика взаимодействия для PageListWorkers.xaml
    /// </summary>
    public partial class PageListWorkers : Page
    {
        private List<Worker> workers;
        public PageListWorkers()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Manager.MainTextBlock.Text = "Сотрудники";
            AtelierBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(o => o.Reload());
            workers = AtelierBaseEntities.GetContext().Worker.ToList();
            DGridWorkers.ItemsSource = workers;
            tbSearch.Focus();
            Manager.BtnBack.Visibility = Visibility.Hidden;
            this.FontFamily = new FontFamily("Cambria");
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            if (cmbSort != null && tbSearch != null && cmbFilter!=null)
            {
                switch (cmbFilter.SelectedIndex)
                {
                    case 0:
                        workers = AtelierBaseEntities.GetContext().Worker.ToList();
                        break;
                    default:
                        workers = AtelierBaseEntities.GetContext().Worker.Where(w=>w.IdType==cmbFilter.SelectedIndex).ToList();
                        break;
                }
                if (tbSearch.Text.Length != 0)
                {
                    workers = workers.Where(w => w.LastName.ToLower().Contains(tbSearch.Text.ToLower()) || w.FirstName.ToLower().Contains(tbSearch.Text.ToLower())
                    || w.Patronymic.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
                }
                if (cmbSort.SelectedIndex == 0)
                {
                    workers = workers.OrderBy(w => w.Id).ToList();
                }
                else if (cmbSort.SelectedIndex == 1)
                {
                    workers = workers.OrderBy(w => w.LastName).ToList();
                }
                DGridWorkers.ItemsSource = workers;
                if (DGridWorkers.Items.Count == 0)
                {
                    DGridWorkers.Visibility = Visibility.Hidden;
                    tblNoResult.Visibility = Visibility.Visible;
                }
                else
                {
                    DGridWorkers.Visibility = Visibility.Visible;
                    tblNoResult.Visibility = Visibility.Hidden;
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var workersForRemoving = DGridWorkers.SelectedItems.Cast<Worker>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {workersForRemoving.Count()} элементов?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    AtelierBaseEntities.GetContext().Worker.RemoveRange(workersForRemoving);
                    AtelierBaseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                    workers = AtelierBaseEntities.GetContext().Worker.ToList();
                    DGridWorkers.ItemsSource = workers;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("С удаляемыми данными имеются связанные записи в других таблицах.", "Удаление отменено!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageWorker(null));
        }
    }
}
