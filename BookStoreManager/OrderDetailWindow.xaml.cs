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
            orderDetails = _orderDao.GetOrderDetails(orderId);
            productDataGrid.ItemsSource = orderDetails;
            
        }


        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void DeleteProductBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveOrderDetailBtn_Click(object sender, RoutedEventArgs e)
        {
           
            // Get the updated values from the UI
            string newCustomerName =  order.CustomerName;
            DateTime newOrderDate = order.OrderDate;
            int newTotalPrice = order.Price;
                

            // Update the order in the database
            _orderDao.UpdateOrder(orderId, newCustomerName, newOrderDate,newTotalPrice);

            this.Close();
           
        }
    }
}
