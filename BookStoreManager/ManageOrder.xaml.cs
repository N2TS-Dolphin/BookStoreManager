using BookStoreManager.Database;
using BookStoreManager.DataType;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookStoreManager
{
    /// <summary>
    /// Interaction logic for ManageOrder.xaml
    /// </summary>
    public partial class ManageOrder : Window
    {
        private OrderDao _orderDao;
        BindingList<OrderModel> orders = new BindingList<OrderModel>();

        int _currentPage = 1;
        int _totalPages = -1;
        DateTime? fromDate = null;
        DateTime? toDate = null;

        public ManageOrder()
        {
            InitializeComponent();
            this.Language = XmlLanguage.GetLanguage("vi-VN");
            _orderDao = new OrderDao();
            LoadPage();
        }

        private void LoadAllOrders()
        {
            // Call the GetOrders method from OrderDao to retrieve orders
            BindingList<OrderModel> orders = _orderDao.GetAllOrders();

            // Bind the retrieved orders to the DataGrid
            OrderDataGrid.ItemsSource = orders;
        }

        private void FilterBtn_Click(object sender, RoutedEventArgs e)
        {
            fromDate = FromDatePicker.SelectedDate;
            toDate = ToDatePicker.SelectedDate;
            _currentPage = 1;

            LoadPage(fromDate, toDate);
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            AddOrderWindow addOrderWindow = new AddOrderWindow();
            addOrderWindow.ShowDialog();
            LoadPage(fromDate, toDate);
        }


        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.SelectedItem != null)
            {
                OrderModel selectedOrder = (OrderModel)OrderDataGrid.SelectedItem;
                int orderId = selectedOrder.OrderId;

               
                OrderDetailWindow orderDetailWindow = new OrderDetailWindow(orderId);
                orderDetailWindow.ShowDialog();
                LoadPage(fromDate, toDate);
            }
            else
            {
                MessageBox.Show("Please select an order to update.");
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.SelectedItem != null)
            {

                OrderModel selectedOrder = (OrderModel)OrderDataGrid.SelectedItem;


                var res = System.Windows.MessageBox.Show($"Are you sure to delete this order: {selectedOrder.OrderId} - {selectedOrder.CustomerName}?", "Delete order", MessageBoxButton.YesNo, MessageBoxImage.Question);


                if (res == MessageBoxResult.Yes)
                {

                    int orderId = selectedOrder.OrderId;

                    _orderDao.DeleteOrder(orderId);

                    LoadPage(fromDate, toDate);
                }
            }
            else
            {
                // If no item is selected, display a message or handle it accordingly
                System.Windows.MessageBox.Show("Please select an order to delete.");
            }
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = (_currentPage-- <= 1) ? 1 : _currentPage;
            LoadPage(fromDate, toDate);
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = (_currentPage++ >= _totalPages) ? _totalPages : _currentPage;
            LoadPage(fromDate, toDate);
        }


        private TargetType GetParent<TargetType>(DependencyObject o) where TargetType : DependencyObject
        {
            if (o == null || o is TargetType) return (TargetType)o;
            return GetParent<TargetType>(VisualTreeHelper.GetParent(o));
        }
        private void DetailBtn_Click(object sender, RoutedEventArgs e)
        {
            var row = GetParent<DataGridRow>((Button)sender);
            int index = OrderDataGrid.Items.IndexOf(row.Item);
            if (index != -1)
            {
                OrderModel selectedOrder = (OrderModel)OrderDataGrid.SelectedItem;
                int orderId = selectedOrder.OrderId;


                OrderDetailWindow orderDetailWindow = new OrderDetailWindow(orderId);
                orderDetailWindow.ShowDialog();
                LoadPage(fromDate, toDate);
            }
            else
            {
                // If no order is selected, display a message or handle it accordingly
                MessageBox.Show("Please select an order to update.");
            }
        }


        private void LoadPage(DateTime? fromDate = null, DateTime? toDate = null)
        {

            var (items, totalItems, totalPages) = _orderDao.GetAllPaging(_currentPage, 10, fromDate, toDate);
            orders = items;
            _totalPages = totalPages;
            if (totalPages == 0)
            {
                OrderDataGrid.ItemsSource = orders;
                txtItemPage.Text = $"{_currentPage}/1";
                return;
            }
            OrderDataGrid.ItemsSource = orders;
            txtItemPage.Text = $"{_currentPage}/{totalPages}";
        }


        private void ReloadPage(object sender, MouseButtonEventArgs e)
        {
            FromDatePicker.SelectedDate = null;
            ToDatePicker.SelectedDate = null;
            fromDate = null;
            toDate = null;
            LoadPage();
        }
    }

    
}
