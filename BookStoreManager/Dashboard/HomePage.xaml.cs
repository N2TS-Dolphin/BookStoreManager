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

using BookStoreManager.Database;
using BookStoreManager.DataType;

namespace BookStoreManager
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        RevenueDao revenueDao = new RevenueDao();
        private List<string> _months = new List<string>() { "January", "Febrary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        public string[] Labels { get; set; }
        public SeriesCollection RevenueSeriesCollection { get; set; }
        public SeriesCollection OrderSeriesCollection { get; set; }

        private int _MonthSelected;
        private int _YearSelected;

        List<int> revenues = new List<int>();
        List<string> days = new List<string>();
        List<int> quantitys = new List<int>();

        public HomePage()
        {
            InitializeComponent();

            revenueDao.Revenues = revenueDao.GetRevenues();

            RevenueSeriesCollection = new SeriesCollection();
            OrderSeriesCollection = new SeriesCollection();
        }

        /// <summary>
        /// Truyền dữ liệu doanh thu từ Database vào Chart
        /// </summary>
        /// <param name="Revenue">Danh sách doanh thu theo ngày</param>
        /// <param name="days">Danh sách ngày</param>
        private void LoadChart(List<int> Quantity, List<int> Revenue, List<string> days)
        {
            // Đặt title cho trục X
            Labels = days.ToArray();

            RevenueSeriesCollection.Add(
                new LineSeries
                {
                    Title = "Revenue",
                    Values = new ChartValues<int>(Revenue), // Set dữ liệu cho từng điểm trong đồ thị
                    LabelPoint = point => point.Y.ToString("#,##0") + " VND", // Format hiển thị trên từng điểm của đồ thị
                }
            );

            OrderSeriesCollection.Add(
                new LineSeries
                {
                    Title = "Order",
                    Values = new ChartValues<int>(Quantity), // Set dữ liệu cho từng điểm trong đồ thị
                    LabelPoint = point => point.Y.ToString("#,##0"), // Format hiển thị trên từng điểm của đồ thị
                }
            );

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonthYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (comboBox.SelectedIndex == 0)
            {
                Month.Visibility = Visibility.Visible;
                Year.Visibility = Visibility.Collapsed;
            }
            else
            {
                Month.Visibility = Visibility.Collapsed;
                Year.Visibility = Visibility.Visible;
            }
        }

        private void Month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            _MonthSelected = comboBox.SelectedIndex;

            // Thay đổi Title Chart
            Total_Order.AxisX[0].Title = _months[_MonthSelected].ToString();
            Total_Revenue.AxisX[0].Title = _months[_MonthSelected].ToString();

            quantitys.Clear();
            revenues.Clear();
            days.Clear();
            RevenueSeriesCollection.Clear();
            OrderSeriesCollection.Clear();

            foreach (var revenue in revenueDao.Revenues)
            {
                if(revenue.OrderDate.Month == _MonthSelected + 1)
                {
                    quantitys.Add(revenue.Quantity);
                    revenues.Add(revenue.Revenue);
                    days.Add(revenue.OrderDate.Day.ToString());
                }
            }


            LoadChart(quantitys, revenues, days);
            DataContext = this;
        }
    }
}
