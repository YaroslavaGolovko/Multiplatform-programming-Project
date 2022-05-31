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
    /// Логика взаимодействия для ControlClient.xaml
    /// </summary>
    public partial class ControlClient : UserControl
    {
        private Client client;
        public ControlClient()
        {
            InitializeComponent();
        }
        public ControlClient(Client client)
        {
            InitializeComponent();
            DataContext = client;
            this.client = client;
        }
        public Client GetContext()
        {
            return client;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageClient((sender as Button).DataContext as Client));
        }
    }
}
