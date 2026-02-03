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
    /// Логика взаимодействия для PageService.xaml
    /// </summary>
    public partial class PageService : Page
    {
        private Service _currentService = new Service();
        public PageService(Service selectedService)
        {
            InitializeComponent();
            if (selectedService != null)
            {
                _currentService = selectedService;
            }
            DataContext = _currentService;
            this.FontFamily = new FontFamily("Cambria");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_currentService.Id != 0)
                Manager.MainTextBlock.Text = "Редактирование услуги";
            else
                Manager.MainTextBlock.Text = "Новая услуга";
            Manager.BtnBack.Visibility = Visibility.Visible;
            tbCost.Text = (Math.Round(_currentService.Cost)).ToString();
            tbName.Focus();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveService();
        }

        private void SaveService()
        {
            if (!Manager.CheckInputData(tbName.Text, tbCost.Text))
            {
                MessageBox.Show("Проверьте правильность вводимых данных!",
                    "Ошибка добавления!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (_currentService.Id == 0)
            {
                AtelierBaseEntities.GetContext().Service.Add(_currentService);
            }

            try
            {
                AtelierBaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!", "Успешно!",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                AtelierBaseEntities.GetContext().Service.Remove(_currentService);
            }
        }
    }
}
