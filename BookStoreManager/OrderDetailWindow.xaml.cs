using BookStoreManager.Database;
using BookStoreManager.Process;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace BookStoreManager
{
    /// <summary>
    /// Interaction logic for OrderDetailWindow.xaml
    /// </summary>
    public partial class OrderDetailWindow : Window
    {

        private BindingList<OrderDetailModel> orderDetails;
        private int orderId;
        private OrderDetailBus orderDetailBus;

        private OrderModel order;

        public OrderDetailWindow(int orderId)
        {
            InitializeComponent();
            this.orderId = orderId;
            orderDetailBus = new OrderDetailBus();
            LoadOrderDetails();
        }

        private void LoadOrderDetails()
        {
            order = orderDetailBus.GetOrderById(orderId);
            if (order != null)
            {
                // Bind data of order (customer name, order date, total price)
                this.DataContext = order;
            }

            // Retrieve order details
            orderDetails = orderDetailBus.GetOrderDetails(orderId);
            productDataGrid.ItemsSource = orderDetails;
        }

        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddBookOrderDetailWindow(orderId);
            var result = screen.ShowDialog();

            if (result == true)
            {
                var newOrderDetail = screen._OrderDetail;
                var existingOrderDetail = orderDetails.FirstOrDefault(detail => detail.Book.BookID == newOrderDetail.Book.BookID);
                if (existingOrderDetail != null)
                {
                    MessageBox.Show("This book already exists in the Order Detail, please edit the quantity if needed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    orderDetails.Add(newOrderDetail);
                    UpdateTotalPrice();
                }
            }
        }

        private void DeleteProductBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrderDetail = (OrderDetailModel)productDataGrid.SelectedItem;

            if (selectedOrderDetail != null)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete this item: " +
                    $"{selectedOrderDetail.Book.BookID} - {selectedOrderDetail.Book.BookName} ?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    orderDetails.Remove(selectedOrderDetail);
                    UpdateTotalPrice();
                }
            }
        }

        private void UpdateProductBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrderDetail = (OrderDetailModel)productDataGrid.SelectedItem;

            if (selectedOrderDetail != null)
            {
                var screen = new AddBookOrderDetailWindow(orderId, selectedOrderDetail);
                var result = screen.ShowDialog();

                if (result == true)
                {
                    var existingOrderDetail = orderDetails.FirstOrDefault(detail => detail.Book.BookID == screen._OrderDetail.Book.BookID);
                    if (existingOrderDetail != null && existingOrderDetail != selectedOrderDetail)
                    {
                        MessageBox.Show("This book already exists in the Order Detail, please edit the quantity if needed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        orderDetails.Remove(selectedOrderDetail);
                        orderDetails.Add(screen._OrderDetail);
                        UpdateTotalPrice();
                    }
                }
            }
        }

        private void UpdateTotalPrice()
        {
            int totalPrice = orderDetails.Sum(detail => detail.Book.Price * detail.Quantity);
            order.Price = totalPrice;
            this.DataContext = null;
            this.DataContext = order;
        }

        private void SaveOrderDetailBtn_Click(object sender, RoutedEventArgs e)
        {
            string newCustomerName = order.CustomerName;
            DateTime newOrderDate = order.OrderDate;
            int newTotalPrice = order.Price;

            orderDetailBus.UpdateOrder(orderId, newCustomerName, newOrderDate, newTotalPrice);
            orderDetailBus.SaveNewOrderItems(orderId, orderDetails);
            DialogResult = true;
        }

        private void CancelOrderDetailBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
