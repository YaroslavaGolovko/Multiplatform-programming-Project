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
    /// Логика взаимодействия для ControlTextile.xaml
    /// </summary>
    public partial class ControlTextile : UserControl
    {
        private Textile textile;
        public ControlTextile()
        {
            InitializeComponent();
        }
        public ControlTextile(Textile textile)
        {
            InitializeComponent();
            DataContext = textile;
            this.textile = textile;
        }
        public Textile GetContext()
        {
            return textile;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageTextile((sender as Button).DataContext as Textile));
        }
    }
}
