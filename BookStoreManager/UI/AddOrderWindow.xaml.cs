using BookStoreManager.Database;
using BookStoreManager.Process;
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

namespace BookStoreManager.UI
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
                MessageBox.Show("Vui lòng nhập đủ thông tin.", "Thiếu Thông Tin", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            // Create an instance of OrderBus
            OrderBus orderBus = new OrderBus();

            // Call the AddOrder method on the OrderBus instance
            orderBus.AddOrder(customerName, orderDate.Value);

            MessageBox.Show("Thêm đơn hàng thành công.", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);

            // Close the AddOrderWindow
            this.Close();
        }
    }
}
