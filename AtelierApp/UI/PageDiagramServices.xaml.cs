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
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AtelierApp.UI
{
    /// <summary>
    /// Логика взаимодействия для PageDiagramServices.xaml
    /// </summary>
    public partial class PageDiagramServices : Page
    {
        private AtelierBaseEntities _context = new AtelierBaseEntities();
        public PageDiagramServices()
        {
            InitializeComponent();
            chartServices.ChartAreas.Add(new ChartArea("Main"));
            var currentSeries = new Series("Количество заказов")
            {
                IsValueShownAsLabel = true
            };
            chartServices.Series.Add(currentSeries);
            cmbType.Items.Add(SeriesChartType.StackedColumn);
            cmbType.Items.Add(SeriesChartType.Doughnut);
            cmbType.Items.Add(SeriesChartType.Pie);
            cmbType.Items.Add(SeriesChartType.Bar);
            cmbType.Items.Add(SeriesChartType.Pyramid);
            cmbType.Items.Add(SeriesChartType.Funnel);
            cmbType.Items.Add(SeriesChartType.Column);
        }

        private void CmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateChart();
        }

        private void DateStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dateEnd.SelectedDate < dateStart.SelectedDate)
            {
                MessageBox.Show("Вы не можете задать начало периода позже окончания периода!", "Необходимо изменить даты", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            UpdateChart();
            Manager.MainTextBlock.Text = "Популярные услуги с " + dateStart.SelectedDate.GetValueOrDefault().ToString("MM/dd/yyyy") +
                " по " + dateEnd.SelectedDate.GetValueOrDefault().ToString("MM/dd/yyyy");
        }

        private void DateEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dateEnd.SelectedDate < dateStart.SelectedDate)
            {
                MessageBox.Show("Вы не можете задать начало периода позже окончания периода!", "Необходимо изменить даты", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            UpdateChart();
            Manager.MainTextBlock.Text = "Популярные услуги с " + dateStart.SelectedDate.GetValueOrDefault().ToString("MM/dd/yyyy") +
                " по " + dateEnd.SelectedDate.GetValueOrDefault().ToString("MM/dd/yyyy");
        }
        private void UpdateChart()
        {
            if (cmbType.SelectedItem is SeriesChartType currentType)
            {
                var currentSeries = chartServices.Series.FirstOrDefault();
                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();
                var listServices = _context.Service.ToList();
                foreach (var service in listServices)
                {
                    int amount = _context.Order.Count(o => o.IdService == service.Id
                    && o.RegistrationDate >= dateStart.SelectedDate && o.RegistrationDate <= dateEnd.SelectedDate);
                    service.NumberOfUses = amount;
                }
                listServices = listServices.OrderByDescending(s => s.NumberOfUses).Take(8).TakeWhile(s => s.NumberOfUses > 0).ToList();
                foreach (var service in listServices)
                {
                    currentSeries.Points.AddXY(service.Title, service.NumberOfUses);
                }
                if (listServices.Count == 0)
                {
                    chartServices.Hide();
                    tblNoResult.Visibility = Visibility.Visible;
                }
                else
                {
                    chartServices.Show();
                    tblNoResult.Visibility = Visibility.Hidden;
                }
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dateStart.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dateEnd.SelectedDate = DateTime.Today;
            dateEnd.DisplayDateEnd = DateTime.Today;
            dateStart.DisplayDateEnd = DateTime.Today;
            Manager.MainTextBlock.Text = "Популярные услуги с " + dateStart.SelectedDate.GetValueOrDefault().ToString("MM/dd/yyyy") +
                " по " + dateEnd.SelectedDate.GetValueOrDefault().ToString("MM/dd/yyyy");
            Manager.BtnBack.Visibility = Visibility.Visible;
            this.FontFamily = new FontFamily("Cambria");
        }
    }
}
