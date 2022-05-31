using AtelierApp.Data;
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
    /// Логика взаимодействия для PageMenu.xaml
    /// </summary>
    public partial class PageMenu : Page
    {
        public PageMenu()
        {
            InitializeComponent();
            this.FontFamily = new FontFamily("Cambria");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Services.Manager.MainTextBlock.Text = "Меню администратора";
            Services.Manager.BtnBack.Visibility = Visibility.Hidden;
        }
    }
}
