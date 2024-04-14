using BookStoreManager.DataType;
using BookStoreManager.Process;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
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

namespace BookStoreManager.UI
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        public BindingList<CustomerModel> customers {  get; set; }
        public CustomerBus Bus { get; set; }
        public CustomerWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Bus = new CustomerBus();
            LoadCustomerList();
        }
        public void LoadCustomerList()
        {
            var(customers, totalPages, currentPage) = Bus.GetCustomerList();
            pageTB.Text = $"{currentPage}/{totalPages}";
            CustomerDataGrid.ItemsSource = customers;
        }
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            var search = searchTB.Text;
            Bus.SearchCustomer(search);
            LoadCustomerList();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddCustomerWindow window = new AddCustomerWindow();
            window.Closed += ManageCustomerWindow_Closed;
            window.Show();
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = CustomerDataGrid.SelectedItem as CustomerModel;
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn 1 khách hàng.");
                return;
            }
            UpdateCustomerWindow window = new UpdateCustomerWindow((CustomerModel)selected.Clone());
            window.Closed += ManageCustomerWindow_Closed;
            window.Show();
        }
        private void ManageCustomerWindow_Closed(object sender, EventArgs e)
        {
            LoadCustomerList();
        }
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = CustomerDataGrid.SelectedItem as CustomerModel;
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn 1 khách hàng.");
                return;
            }
            var messbox = MessageBox.Show($"Bạn có chắc muốn xóa khách hàng {selected.CustomerName}, id = {selected.CustomerID}?"
                , $"Xóa khách hàng {selected.CustomerName}?", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (messbox == MessageBoxResult.OK)
            {
                try
                {
                    CustomerManagerBus bus = new CustomerManagerBus();
                    bus.DeleteCustomer(selected);
                    MessageBox.Show($"Xóa danh mục {selected.CustomerName} thành công.");
                    Bus.RefreshPage();
                    LoadCustomerList();
                }
                catch (Exception ex) { MessageBox.Show($"Xóa danh mục {selected.CustomerName} thất bại."); }
            }
        }

        private void prevButton_Click(object sender, RoutedEventArgs e)
        {
            Bus.MoveToPreviousPage();
            LoadCustomerList();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            Bus.MoveToNextPage();
            LoadCustomerList();
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            Bus.RefreshPage();
            LoadCustomerList();
        }
    }
}
