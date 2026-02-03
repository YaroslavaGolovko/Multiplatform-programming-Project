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
using Word = Microsoft.Office.Interop.Word;

namespace AtelierApp.UC
{
    /// <summary>
    /// Логика взаимодействия для ControlOrder.xaml
    /// </summary>
    public partial class ControlOrder : UserControl
    {
        private Order order;
        public ControlOrder()
        {
            InitializeComponent();
        }
        public ControlOrder(Order order)
        {
            InitializeComponent();
            DataContext = order;
            this.order = order;
        }
        public Order GetContext()
        {
            return order;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageOrder((sender as Button).DataContext as Order));
        }
    }
}
