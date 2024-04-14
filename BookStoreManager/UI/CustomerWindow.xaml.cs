using BookStoreManager.DataType;
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

        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {

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
