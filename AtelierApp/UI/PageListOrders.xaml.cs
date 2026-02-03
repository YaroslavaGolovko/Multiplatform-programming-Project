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
using Word = Microsoft.Office.Interop.Word;

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
                    MessageBox.Show("При удалении данных возникли неполадки!", "Удаление отменено!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageOrder(null));
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

        private void btnCreateCheck_Click(object sender, RoutedEventArgs e)
        {
            var arrayOrders = DGridOrders.SelectedItems.Cast<Order>().ToList();
            if (arrayOrders.Count == 0)
            {
                MessageBox.Show("Необходимо выделить заказы, по которым будут сформированы чеки!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                int j = 1;
                foreach (Order order in arrayOrders)
                {
                    if (order.IsCompleted == true)
                    {
                        var number = order.Id.ToString();
                        var client = order.Client.LastName.ToString() + " " + order.Client.FirstName.ToString();
                        var worker = order.Worker.LastName.ToString() + " " + order.Worker.FirstName.ToString();
                        var service = order.Service.Title.ToString();
                        var serviceCost = order.Service.Cost.ToString("0.00");
                        var textile = order.Textile.Title.ToString();
                        var textileCost = order.Textile.Cost.ToString("0.00");
                        var regDate = order.RegistrationDate.ToString("MM/dd/yyyy");
                        string finalDate = order.CompletionDate?.ToString("MM/dd/yyyy");
                        var sum = order.Price.ToString("0.00");
                        var admin = AtelierBaseEntities.GetContext().Worker.Where(w => w.Login == Authorization.userLogin).FirstOrDefault();
                        var adminName = admin.LastName.ToString() + " " + admin.FirstName.ToString();
                        Word.Application app = new Word.Application();
                        Word.Document doc = app.Documents.Add(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\" + "Resources/template.dotx");
                        try
                        {
                            object source = "check.docx"; ;
                            doc.Activate();

                            Word.Bookmarks wBookmarks = doc.Bookmarks;
                            Word.Range wRange;
                            int i = 0;
                            string[] data = new string[11] { adminName, client, sum, finalDate, number, regDate, service, serviceCost, textile, textileCost, worker };
                            foreach (Word.Bookmark mark in wBookmarks)
                            {
                                wRange = mark.Range;
                                wRange.Text = data[i];
                                i++;
                            }
                            doc.SaveAs2($@"D:\Ателье_Рада_Best_Чеки\чек{order.Client.LastName.ToString()}Заказ№{order.Id}_{j}.pdf", Word.WdExportFormat.wdExportFormatPDF);
                            app.Visible = true;
                            Microsoft.Office.Interop.Word.Dialog printDialog = app.Dialogs[Microsoft.Office.Interop.Word.WdWordDialog.wdDialogFilePrint];
                            if (printDialog.Show() == 1)
                            {
                                doc.PrintOut();
                            }
                            doc.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges);

                            doc = null;
                        }
                        catch (Exception ex)
                        {
                            doc.Close();
                            doc = null;
                            MessageBox.Show("Во время выполнения произошла ошибка!");

                        }
                        j++;
                        MessageBox.Show("Чек успешно создан и сохранен на диске D в папке Ателье_Рада_Best_Чеки!");
                    }
                    else {
                        MessageBox.Show("Заказ не является выполненным, поэтому формирование чека недоступно!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    Manager.MainFrame.Navigate(new PageListOrders());
                }
            }
        }
    }
}
