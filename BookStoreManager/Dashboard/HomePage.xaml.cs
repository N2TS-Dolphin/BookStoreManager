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
using System.DirectoryServices;
using System.Collections.ObjectModel;
using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        private int _MonthSelected, _YearSelected, _currentPage = 0, _totalPage = 0;
        private const int _pageSize = 10;
        private DataGridColumn _currentSortColumn;
        private ListSortDirection _currentSortDirection = ListSortDirection.Ascending;
        private List<ProductRankingModel> _originalData;

        public string CompareOrder { get; set; }
        public string CompareRevenue { get; set; }
        public string TotalRevenueCurMonth { get; set; }
        public string TotalOrderCurMonth { get; set; }
        public List<string> Labels { get; set; }
        public List<ProductRankingModel> TableData { get; set; }
        public SeriesCollection RevenueSeriesCollection { get; set; }
        public SeriesCollection OrderSeriesCollection { get; set; }

        List<int> revenues = new List<int>();
        List<int> quantitys = new List<int>();

        public HomePage()
        {
            InitializeComponent();


        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Set dữ liệu cho các biến toàn cục
            RevenueSeriesCollection = new SeriesCollection();
            OrderSeriesCollection = new SeriesCollection();
            Labels = new List<string>();
            _originalData = productDao.getRankingList();

            // Format dữ liệu doanh thu theo định dạng
            foreach (var item in _originalData)
            {
                item.RevenueFormatted = item.Revenue.ToString("#,##0"); // Format dạng số nguyên có dấu phân cách ngàn
            }
            _totalPage = _originalData.Count() / _pageSize;

            // Format dữ liệu só lượng và doanh thu theo tháng theo định dạng
            foreach (var data in revenueDao.GetRevenuesByMonth())
            {
                if (data.Month == DateTime.Now.Month && data.Year == DateTime.Now.Year)
                {
                    TotalOrderCurMonth = data.Quantity.ToString("#,##0");
                    TotalRevenueCurMonth = data.Revenue.ToString("#,##0");
                }
            }

            UpdatePagination();
            CompareData();

            DataContext = this;
        }

        /// <summary>
        /// Truyền dữ liệu doanh thu từ Database vào Chart
        /// </summary>
        /// <param name="Quantity">Danh sách số lượng theo ngày</param>
        /// <param name="Revenue">Danh sách doanh thu theo ngày</param>
        /// <param name="Temps">Danh sách ngày</param>
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
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void IncomeCurMonth(List<RevenueModel> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Month == DateTime.Now.Month && data[i].Year == DateTime.Now.Year)
                {
                    TotalOrderCurMonth = data[i].Quantity.ToString("#,##0");
                    TotalRevenueCurMonth = data[i].Revenue.ToString("#,##0");
                }
            }

            DataContext = this;
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

        /// <summary>
        /// Chức năng quay lại trang trước
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prevButton_click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 0)
            {
                _currentPage--;
                UpdatePagination();
            }
        }

        /// <summary>
        /// Chức năng qua trang tiếp theo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nextButton_click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _originalData.Count / _pageSize)
            {
                _currentPage++;
                UpdatePagination();
            }
        }

        /// <summary>
        /// Cập nhật bảng theo trang
        /// </summary>
        private void UpdatePagination()
        {
            int startIndex = _currentPage * _pageSize;
            var dataForCurPage = _originalData.Skip(startIndex).Take(_pageSize).ToList();
            txtItemPage.Text = $"{_currentPage + 1}/{_totalPage + 1}";

            if (_currentPage == _totalPage)
                nextButton.IsEnabled = false;
            else
                nextButton.IsEnabled = true;
            if (_currentPage == 0)
                prevButton.IsEnabled = false;
            else
                prevButton.IsEnabled = true;

            RankingTable.ItemsSource = dataForCurPage;
        }

        /// <summary>
        /// Chức năng sắp xếp của bảng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RankingTable_Sorting(object sender, DataGridSortingEventArgs e)
        {
            DataGridColumn column = e.Column;

            bool isSameColumn = _currentSortColumn == column;

            // Đảo hướng sắp xếp nếu cùng một cột đang được sắp xếp lại
            if (isSameColumn)
            {
                _currentSortDirection = _currentSortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            }
            else
            {
                _currentSortDirection = ListSortDirection.Ascending;
            }

            _currentSortColumn = column;

            // Xóa sắp xếp trước đó
            foreach (var col in RankingTable.Columns)
            {
                col.SortDirection = null;
            }

            // Thiết lập cột và hướng sắp xếp mới
            column.SortDirection = _currentSortDirection;

            // Sắp xếp dữ liệu
            if (column.Header.ToString() == "Doanh thu")
            {
                if (_currentSortDirection == ListSortDirection.Ascending)
                {
                    _originalData.Sort((x, y) => x.Revenue.CompareTo(y.Revenue));
                }
                else
                {
                    _originalData.Sort((x, y) => y.Revenue.CompareTo(x.Revenue));
                }
            }
            else if (column.Header.ToString() == "Số lượng")
            {
                if (_currentSortDirection == ListSortDirection.Ascending)
                {
                    _originalData.Sort((x, y) => x.Quantity.CompareTo(y.Quantity));
                }
                else
                {
                    _originalData.Sort((x, y) => y.Quantity.CompareTo(x.Quantity));
                }
            }

            UpdatePagination();
        }
    }
}
