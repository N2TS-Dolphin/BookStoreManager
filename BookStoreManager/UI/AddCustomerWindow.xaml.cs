using BookStoreManager.DataType;
using BookStoreManager.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookStoreManager.UI
{
    /// <summary>
    /// Interaction logic for AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window
    {
        public CustomerModel customer { get; set; }
        public CustomerManagerBus Bus { get; set; }
        public AddCustomerWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Bus = new CustomerManagerBus();
            customer = new CustomerModel();
            DataContext = customer;
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            if(customer.CustomerName == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng.");
                return;
            }
            string emailPattern = @"^([a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6})$";
            Regex emailRegex = new Regex(emailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!emailRegex.IsMatch(customer.CustomerEmail) && customer.CustomerEmail != string.Empty)
            {
                MessageBox.Show("Email không hợp lệ.");
                return;
            }
            
            string phonePattern = @"^0\d{9}$";
            Regex phoneRegex = new Regex(phonePattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!phoneRegex.IsMatch(customer.CustomerPhone) && customer.CustomerEmail != string.Empty)
            {
                MessageBox.Show("Số điện thoại không hợp lệ.");
                return;
            }
            try
            {
                Bus.AddCustomer((CustomerModel)customer.Clone());
                MessageBox.Show($"Thêm khách hàng {customer.CustomerName} thành công.");
            }
            catch (Exception ex) { MessageBox.Show($"Thêm khách hàng {customer.CustomerName} thất bại."); }
        }

        private void quitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
