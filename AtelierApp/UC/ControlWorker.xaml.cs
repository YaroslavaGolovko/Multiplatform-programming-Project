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
    /// Логика взаимодействия для ControlWorker.xaml
    /// </summary>
    public partial class ControlWorker : UserControl
    {
        private Worker worker;
        public ControlWorker()
        {
            InitializeComponent();
        }
        public ControlWorker(Worker worker)
        {
            InitializeComponent();
            DataContext = worker;
            this.worker = worker;
        }
        public Worker GetContext()
        {
            return worker;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageWorker((sender as Button).DataContext as Worker));
        }
    }
}
