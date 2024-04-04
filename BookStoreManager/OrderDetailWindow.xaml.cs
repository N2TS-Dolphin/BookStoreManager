using BookStoreManager.Database;
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
        private OrderDetailDao _orderDetailDao = new OrderDetailDao();
        private OrderDao _orderDao = new OrderDao();

        private OrderModel order;

        public OrderDetailWindow(int orderId)
        {
            InitializeComponent();
            this.orderId = orderId;
            LoadOrderDetails();
        }

        private void LoadOrderDetails()
        {
            order = _orderDao.GetOrderById(orderId);
            if (order != null)
            {
                //make datacontext to bind data of order (customername,orderdate,totalprice)
                this.DataContext = order;
            }

            // Retrieve order details
            orderDetails = _orderDetailDao.GetOrderDetails(orderId);
            productDataGrid.ItemsSource = orderDetails;
            
        }


        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddBookOrderDetail(orderId);
            var result = screen.ShowDialog(); // Show the AddBookOrderDetail window as a dialog

            if (result == true) // Check if the dialog was closed with DialogResult set to true
            {
                // Retrieve the OrderDetailModel from the AddBookOrderDetail window
                var newOrderDetail = screen._OrderDetail;

                // Check if the book already exists in orderDetails
                var existingOrderDetail = orderDetails.FirstOrDefault(detail => detail.Book.BookID == newOrderDetail.Book.BookID);
                if (existingOrderDetail != null)
                {
                    // Show a message box indicating that the book already exists
                    MessageBox.Show("This book already exists in the Order Detail, please edit the quantity if needed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // Add the new order detail to the orderDetails list
                    orderDetails.Add(newOrderDetail);
                    UpdateTotalPrice();
                }
            }
        }

        private void UpdateTotalPrice()
        {
            // Calculate total price based on order details
            int totalPrice = orderDetails.Sum(detail => detail.Book.Price * detail.Quantity);
            // Update the OrderModel with the new total price
            order.Price = totalPrice;

            // Update UI bindings
            this.DataContext = null; // Reset DataContext to trigger re-binding
            this.DataContext = order; // Re-bind order to update UI
        }

        private void DeleteProductBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrderDetail = (OrderDetailModel)productDataGrid.SelectedItem;

            if (selectedOrderDetail != null)
            {
                // Confirm with the user before deleting
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete this item: " +
                    $"{selectedOrderDetail.Book.BookID} - {selectedOrderDetail.Book.BookName} ?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Remove the selected order detail from the list
                    orderDetails.Remove(selectedOrderDetail);

                    // Update the total price
                    UpdateTotalPrice();
                }
            }
        }
        private void UpdateProductBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrderDetail = (OrderDetailModel)productDataGrid.SelectedItem;

            if (selectedOrderDetail != null)
            {
                var screen = new AddBookOrderDetail(orderId, selectedOrderDetail);
                var result = screen.ShowDialog();

                if (result == true)
                {
                    // Check if the book already exists in orderDetails
                    var existingOrderDetail = orderDetails.FirstOrDefault(detail => detail.Book.BookID == screen._OrderDetail.Book.BookID);
                    if (existingOrderDetail != null && existingOrderDetail != selectedOrderDetail)
                    {
                        MessageBox.Show("This book already exists in the Order Detail, please edit the quantity if needed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        // Remove the existing OrderDetailModel from the BindingList
                        orderDetails.Remove(selectedOrderDetail);

                        // Add the new OrderDetailModel returned from the AddBookOrderDetail window
                        orderDetails.Add(screen._OrderDetail);

                        UpdateTotalPrice();
                    }
                }
            }
        }
        

        private void SaveOrderDetailBtn_Click(object sender, RoutedEventArgs e)
        {

            // Get the updated values
            string newCustomerName = order.CustomerName;
            DateTime newOrderDate = order.OrderDate;
            int newTotalPrice = order.Price;

            // Update the order in the database
            _orderDao.UpdateOrder(orderId, newCustomerName, newOrderDate, newTotalPrice);

            // Delete existing order items for the orderId
            _orderDetailDao.DeleteOrderItems(orderId);

            // Insert new order items from the BindingList

            _orderDetailDao.InsertOrderItems(orderId, orderDetails);
            DialogResult = true;
           

        }

        private void CancelOrderDetailBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
