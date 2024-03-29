using BookStoreManager.Database;
using BookStoreManager.DataType;
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
using LiveCharts.Wpf;
using LiveCharts;

namespace BookStoreManager
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        OrderDao order = new OrderDao();
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public HomePage()
        {
            InitializeComponent();
            LoadOrderChart();

            DataContext = this;
        }

        private void LoadOrderChart()
        {
            order.orders = order.readOrders();
            List<int> values = new List<int>();
            List<string> days = new List<string>();
            for (int i = 0; i < order.orders.Count; i++)
            {
                values.Add(order.orders[i].Price);
                days.Add(order.orders[i].OrderDate.Day.ToString());
            }

            SeriesCollection = new SeriesCollection()
            {
                new LineSeries
                {
                    Title = "Order",
                    Values = new ChartValues<int>(values),
                    LabelPoint = point => point.Y.ToString("#,##0") + " VND",
                }
            };

            // Đặt title cho trục X
            Total_Order.AxisX[0].Title = "March";
            Labels = days.ToArray();
        }
    }
}
