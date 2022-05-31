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
    /// Логика взаимодействия для PageTextile.xaml
    /// </summary>
    public partial class PageTextile : Page
    {
        private Textile _currentTextile = new Textile();
        public PageTextile(Textile selectedTextile)
        {
            InitializeComponent();
            if (selectedTextile != null)
            {
                _currentTextile = selectedTextile;
                if (selectedTextile.InStock)
                    rbInStock.IsChecked = true;
            }
            DataContext = _currentTextile;
            this.FontFamily = new FontFamily("Cambria");
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (tbName.Text.Length == 0)
                errors.AppendLine("Необходимо указать наименование ткани");
            if (tbCost.Text.Length == 0)
                errors.AppendLine("Необходимо указать стоимость ткани");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentTextile.Id == 0)
            {
                AtelierBaseEntities.GetContext().Textile.Add(_currentTextile);
            }

            if (rbInStock.IsChecked == true)
                _currentTextile.InStock = true;
            else
                _currentTextile.InStock = false;

            try
            {
                AtelierBaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                AtelierBaseEntities.GetContext().Textile.Remove(_currentTextile);
            }
        }

        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            byte[] binary = Manager.SelectImage(imageTextile);
            _currentTextile.Image = binary;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Manager.BtnBack.Visibility = Visibility.Visible;
            if (_currentTextile.Id != 0)
                Manager.MainTextBlock.Text = "Редактирование ткани";
            else
                Manager.MainTextBlock.Text = "Новая ткань";
            tbCost.Text = (Math.Round(_currentTextile.Cost)).ToString();
        }
    }
}
