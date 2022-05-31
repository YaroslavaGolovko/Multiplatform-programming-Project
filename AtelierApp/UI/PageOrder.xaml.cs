using AtelierApp.Data;
using Microsoft.Win32;
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
    /// Логика взаимодействия для PageOrder.xaml
    /// </summary>
    public partial class PageOrder : Page
    {
        private Order _currentOrder = new Order();
        private Textile textile;
        private Service service;
        decimal textileCost;
        decimal serviceCost;
        private int newOrder=0;
        public PageOrder(Order selectedOrder)
        {
            InitializeComponent();
            if (selectedOrder != null)
            {
                _currentOrder = selectedOrder;
                if (selectedOrder.IsCompleted)
                    rbCompleted.IsChecked = true;
                newOrder = 0;
            }
            else
            {
                _currentOrder.RegistrationDate = DateTime.Today;
                _currentOrder.Price = 0;
                textileCost = 0;
                serviceCost = 0;
                newOrder = 1;
                
            }
            if (rbNotCompleted.IsChecked == true)
            {
                tblCompletionDate.Visibility = Visibility.Hidden;
            }
            DataContext = _currentOrder;
            this.FontFamily = new FontFamily("Cambria");
            cbClients.ItemsSource = AtelierBaseEntities.GetContext().Client.ToList();
            cbWorkers.ItemsSource = AtelierBaseEntities.GetContext().Worker.ToList().Where(w => w.IdType == 2);
            cbTextile.ItemsSource = AtelierBaseEntities.GetContext().Textile.ToList();
            cbService.ItemsSource = AtelierBaseEntities.GetContext().Service.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (cbClients.SelectedValue == null)
                errors.AppendLine("Необходимо указать клиента");
            if (cbWorkers.SelectedValue == null)
                errors.AppendLine("Необходимо указать работника");
            if (cbTextile.SelectedValue == null)
                errors.AppendLine("Необходимо выбрать ткань");
            if (cbService.SelectedValue == null)
                errors.AppendLine("Необходимо выбрать услугу");
            if (_currentOrder.Textile.InStock == false)
                errors.AppendLine("Выбранной ткани нет в наличии");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (newOrder==1)
            {
                AtelierBaseEntities.GetContext().Order.Add(_currentOrder);
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
                AtelierBaseEntities.GetContext().Order.Remove(_currentOrder);
            }
        }

        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            byte[] binary = Manager.SelectImage(imageOrder);
            if(rbCompleted.IsChecked==true)
                _currentOrder.Image = binary;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Manager.BtnBack.Visibility = Visibility.Visible;
            if (_currentOrder.Id != 0)
                Manager.MainTextBlock.Text = "Редактирование заказа за " + textDate.Text;
            else
                Manager.MainTextBlock.Text = "Новый заказ за " + textDate.Text;
        }

        private void RbCompleted_Checked(object sender, RoutedEventArgs e)
        {
            if (tblCompletionDate != null)
            {
                _currentOrder.IsCompleted = true;
                _currentOrder.CompletionDate = DateTime.Today;
                textcompletionDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
                tblCompletionDate.Visibility = Visibility.Visible;
            }
        }

        private void RbNotCompleted_Checked(object sender, RoutedEventArgs e)
        {
            if (tblCompletionDate != null)
            {
                _currentOrder.IsCompleted = false;
                _currentOrder.CompletionDate = null;
                tblCompletionDate.Visibility = Visibility.Hidden;
            }
        }

        private void CbTextile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetPtice();
        }

        private void CbService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetPtice();
        }
        private void SetPtice()
        {
            if (cbTextile.SelectedItem != null)
                textile = AtelierBaseEntities.GetContext().Textile.FirstOrDefault(t => t.Title == ((Textile)cbTextile.SelectedItem).Title);
            if (cbService.SelectedItem != null)
                service = AtelierBaseEntities.GetContext().Service.FirstOrDefault(s => s.Title == ((Service)cbService.SelectedItem).Title);
            if (textile != null)
                textileCost = textile.Cost;
            if (service != null)
                serviceCost = service.Cost;
            _currentOrder.Price = textileCost + serviceCost;
            textPrice.Text = (Math.Round(_currentOrder.Price)).ToString();
        }
    }
}
