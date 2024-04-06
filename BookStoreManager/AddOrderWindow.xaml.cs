using BookStoreManager.Database;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookStoreManager
{
    /// <summary>
    /// Interaction logic for AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        public AddOrderWindow()
        {
            InitializeComponent();
            this.Language = XmlLanguage.GetLanguage("vi-VN");
        }

        private void AddOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            string customerName = txtCustomerName.Text;
            DateTime? orderDate = dpOrderDate.SelectedDate;

            if (string.IsNullOrWhiteSpace(customerName) || !orderDate.HasValue)
            {
                MessageBox.Show("Please enter the required information.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            // Call a method to add the order to the database
            AddOrderToDatabase(customerName, orderDate.Value);

            MessageBox.Show("New Order added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Close the AddOrderWindow
            this.Close();
        }

        private void AddOrderToDatabase(string customerName, DateTime orderDate)
        {
            OrderDao orderDao = new OrderDao();

            orderDao.AddOrder(customerName, orderDate);
        }
    }
}
