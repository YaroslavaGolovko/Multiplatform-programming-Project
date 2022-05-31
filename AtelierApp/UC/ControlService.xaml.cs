using AtelierApp.Data;
using AtelierApp.UI;
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

namespace AtelierApp.UC
{
    /// <summary>
    /// Логика взаимодействия для ControlService.xaml
    /// </summary>
    public partial class ControlService : UserControl
    {
        private Service service;
        public ControlService()
        {
            InitializeComponent();
        }

        public ControlService(Service service)
        {
            InitializeComponent();
            DataContext = service;
            this.service = service;
        }
        public Service GetContext()
        {
            return service;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageService((sender as Button).DataContext as Service));
        }
    }
}
