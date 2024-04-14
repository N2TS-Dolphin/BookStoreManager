using BookStoreManager.Database;
using BookStoreManager.DataType;
using BookStoreManager.Process;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace BookStoreManager.UI
{
    public partial class AddOrderWindow : Window
    {
        private OrderBus _orderBus = new OrderBus();
        private string _search = "";

        public AddOrderWindow()
        {
            InitializeComponent();
            this.Language = XmlLanguage.GetLanguage("vi-VN");
            Loaded += AddOrderWindow_Loaded;
        }

        private void AddOrderWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the text of customerSearchTB
            customerSearchTB.Text = _search;

            // Load customers into the DataGrid
            CustomerDataGrid.ItemsSource = _orderBus.GetAllCustomers(_search);
        }

        private void AddOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerModel selectedCustomer = (CustomerModel)CustomerDataGrid.SelectedItem;

            if (selectedCustomer == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Get the selected order date from the DatePicker
            DateTime? orderDate = dpOrderDate.SelectedDate;

            if (!orderDate.HasValue)
            {
                MessageBox.Show("Vui lòng chọn ngày đặt đơn.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Add the order using the OrderBus
            _orderBus.AddOrder(selectedCustomer.CustomerID, orderDate.Value);

            MessageBox.Show("Thêm đơn hàng thành công.", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            string searchText = customerSearchTB.Text;

            // Search for customers using the OrderBus
            BindingList<CustomerModel> searchResults = _orderBus.SearchCustomers(searchText);

            // Update the DataGrid with the search results
            CustomerDataGrid.ItemsSource = searchResults;
        }
    }
}
