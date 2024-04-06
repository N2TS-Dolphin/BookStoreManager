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
using BookStoreManager.Process;
using System.ComponentModel;

namespace BookStoreManager
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        RevenueDao revenueDao = new RevenueDao();
        ProductRankingDao productDao = new ProductRankingDao();
        HomePageBus bus = new HomePageBus();

        private List<string> _months = new List<string>() { "January", "Febrary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        public string CompareOrder { get; set; }
        public string CompareRevenue { get; set; }
        public List<string> Labels { get; set; }
        public List<ProductRankingModel> TableData {  get; set; }
        public SeriesCollection RevenueSeriesCollection { get; set; }
        public SeriesCollection OrderSeriesCollection { get; set; }

        private int _MonthSelected;
        private int _YearSelected;
        private bool _isSortAscending = false;

        List<int> revenues = new List<int>();
        List<int> quantitys = new List<int>();

        public HomePage()
        {
            InitializeComponent();

            RevenueSeriesCollection = new SeriesCollection();
            OrderSeriesCollection = new SeriesCollection();
            Labels = new List<string>();

            CompareData();
            Ranking();
        }

        /// <summary>
        /// Truyền dữ liệu doanh thu từ Database vào Chart
        /// </summary>
        /// <param name="Revenue">Danh sách doanh thu theo ngày</param>
        /// <param name="days">Danh sách ngày</param>
        private void LoadChart(List<int> Quantity, List<int> Revenue, List<string> Temps)
        {
            // Thêm dữ liệu vào Chart
            RevenueSeriesCollection.Add(
                new LineSeries
                {
                    Title = "Revenue",
                    Values = new ChartValues<int>(Revenue), // Set dữ liệu cho từng điểm trong đồ thị
                    LabelPoint = point => point.Y.ToString("#,##0") + " VND", // Format hiển thị trên từng điểm của đồ thị
                }
            );

            // Thêm dữ liệu vào Chart
            OrderSeriesCollection.Add(
                new LineSeries
                {
                    Title = "Order",
                    Values = new ChartValues<int>(Quantity), // Set dữ liệu cho từng điểm trong đồ thị
                    LabelPoint = point => point.Y.ToString("#,##0"), // Format hiển thị trên từng điểm của đồ thị
                }
            );

            // Thêm Label cho từng dữ liệu
            Labels.AddRange(Temps);
        }

        /// <summary>
        /// Chọn hiển thị theo tháng trong năm hiện tại hay theo 12 tháng trong năm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonthYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (comboBox.SelectedIndex == 0)
            {
                // Đặt lại giá trị của Revenues sau mỗi lẫn chuyển chế độ xem
                revenueDao.Revenues.Clear();
                revenueDao.Revenues.AddRange(revenueDao.GetRevenuesByDay());

                // Cài đặt hiển thị vùng chọn tháng và ẩn đi vùng nhập năm
                Month.Visibility = Visibility.Visible;
                Year.Visibility = Visibility.Collapsed;
                btn_Year.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Đặt lại giá trị của Revenues sau mỗi lẫn chuyển chế độ xem
                revenueDao.Revenues.Clear();
                revenueDao.Revenues.AddRange(revenueDao.GetRevenuesByMonth());

                // Cài đặt hiển thị vùng nhập năm và ẩn đi vùng chọn tháng
                Month.Visibility = Visibility.Collapsed;
                Year.Visibility = Visibility.Visible;
                btn_Year.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Chọn tháng để hiển thị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> temps = new List<string>();

            ComboBox comboBox = sender as ComboBox;

            _MonthSelected = comboBox.SelectedIndex;

            // Thay đổi Title Chart
            Total_Order.AxisX[0].Title = _months[_MonthSelected].ToString();
            Total_Revenue.AxisX[0].Title = _months[_MonthSelected].ToString();

            // Làm mới giá trị các biến cho mỗi lần load lại
            RevenueSeriesCollection.Clear();
            OrderSeriesCollection.Clear();
            Labels.Clear();
            quantitys.Clear();
            revenues.Clear();

            // Lấy từng revenue trong Revenues thêm vào các biến dữ liệu
            foreach (var revenue in revenueDao.Revenues)
            {
                // Xét dữ liệu đang xét có cùng tháng được chọn và trong năm hiện tại hay không
                if (revenue.OrderDate.Month == _MonthSelected + 1 && revenue.OrderDate.Year == DateTime.Now.Year)
                {
                    quantitys.Add(revenue.Quantity);
                    revenues.Add(revenue.Revenue);
                    temps.Add(revenue.OrderDate.Day.ToString());
                }
            }

            LoadChart(quantitys, revenues, temps);
            DataContext = this;
        }

        /// <summary>
        /// Chọn năm để hiển thị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Year_Click(object sender, RoutedEventArgs e)
        {
            List<string> temps = new List<string>();

            // Thay đổi Title Chart
            Total_Order.AxisX[0].Title = Year.Text;
            Total_Revenue.AxisX[0].Title = Year.Text;

            // Làm mới giá trị các biến cho mỗi lần load lại
            RevenueSeriesCollection.Clear();
            OrderSeriesCollection.Clear();
            Labels.Clear();
            quantitys.Clear();
            revenues.Clear();

            var curMonth = 0;

            // Xét xem dữ liệu đc nhập có phải là kiểu số nguyên và có 4 chữ số
            if (int.TryParse(Year.Text, out _YearSelected) && Year.Text.Length <= 4)
            {
                // Lấy từng revenue trong Revenues thêm vào các biến dữ liệu
                foreach (var revenue in revenueDao.Revenues)
                {
                    // Xét dữ liệu đang xét có cùng năm được chọn không
                    if (revenue.Year == _YearSelected)
                    {
                        // Xét từng tháng, tháng nào có dữ liệu thì thêm vào từ database, không có thì cho bằng 0
                        for (int i = curMonth + 1; i <= revenue.Month; i++)
                        {
                            if (i == revenue.Month)
                            {
                                quantitys.Add(revenue.Quantity);
                                revenues.Add(revenue.Revenue);
                                curMonth = i;
                            }
                            else
                            {
                                quantitys.Add(0);
                                revenues.Add(0);
                            }
                            temps.Add(_months[i - 1]);
                        }
                    }
                }

                // Xét các tháng còn lại nếu sót
                for (int i = curMonth + 1; i <= 12; i++)
                {
                    quantitys.Add(0);
                    revenues.Add(0);
                    temps.Add(_months[i - 1]);
                }

                LoadChart(quantitys, revenues, temps);
                DataContext = this;
            }
            else
            {
                MessageBox.Show("Vui lòng nhập số nguyên có 4 chữ số");
            }
        }

        /// <summary>
        /// So sánh dữ liệu tháng hiện tại với tháng trước nó
        /// </summary>
        private void CompareData()
        {
            CompareOrder = bus.CompareOrder();
            CompareRevenue = bus.CompareRevenue();

            DataContext = this;
        }
    
        private void Ranking()
        {
            RankingTable.ItemsSource = productDao.rankingList();
        }

        private void RankingTable_Sorting(object sender, DataGridSortingEventArgs e)
        {
            DataGridColumn column = e.Column;

            // Đảo chiều sắp xếp
            _isSortAscending = !_isSortAscending;

            // Xóa sắp xếp trước đó
            foreach (var col in RankingTable.Columns)
            {
                col.SortDirection = null;
            }

            // Thiết lập cột và hướng sắp xếp mới
            column.SortDirection = _isSortAscending ? ListSortDirection.Ascending : ListSortDirection.Descending;

            // Sắp xếp dữ liệu
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(RankingTable.ItemsSource);
            collectionView.SortDescriptions.Clear();
            if (_isSortAscending)
            {
                collectionView.SortDescriptions.Add(new SortDescription(column.SortMemberPath, ListSortDirection.Ascending));
            }
            else
            {
                collectionView.SortDescriptions.Add(new SortDescription(column.SortMemberPath, ListSortDirection.Descending));
            }
        }

    }
}
