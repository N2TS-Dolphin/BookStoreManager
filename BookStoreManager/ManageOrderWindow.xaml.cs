using BookStoreManager.Database;
using BookStoreManager.DataType;
using BookStoreManager.Process;
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
    /// Interaction logic for ManageOrderWindow.xaml
    /// </summary>
    public partial class ManageOrderWindow : Window
    {
        private OrderBus orderBus;
        BindingList<OrderModel> orders = new BindingList<OrderModel>();

        public ManageOrderWindow()
        {
            InitializeComponent();
            this.Language = XmlLanguage.GetLanguage("vi-VN");
            orderBus = new OrderBus();
            LoadPage();
        }

        private void LoadPage()
        {
            var (items, totalItems, totalPages, current) = orderBus.GetAllPaging(orderBus.FromDate, orderBus.ToDate);
            orders = items;
            orderBus.TotalPages = totalPages;

         

            if (totalPages == 0)
            {
                OrderDataGrid.ItemsSource = orders;
                txtItemPage.Text = $"{current}/1";
                return;
            }
            OrderDataGrid.ItemsSource = orders;
            txtItemPage.Text = $"{current}/{totalPages}";
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
          
            orderBus.MoveToPreviousPage();
            LoadPage();
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
          
            orderBus.MoveToNextPage();
            LoadPage();
        }

        private void FilterBtn_Click(object sender, RoutedEventArgs e)
        {
            orderBus.FromDate = FromDatePicker.SelectedDate;
            orderBus.ToDate = ToDatePicker.SelectedDate;
            LoadPage();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            AddOrderWindow addOrderWindow = new AddOrderWindow();
            addOrderWindow.ShowDialog();
            LoadPage();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.SelectedItem != null)
            {
                OrderModel selectedOrder = (OrderModel)OrderDataGrid.SelectedItem;
                int orderId = selectedOrder.OrderId;

                OrderDetailWindow orderDetailWindow = new OrderDetailWindow(orderId);
                var result = orderDetailWindow.ShowDialog();
                if (result == true)
                {
                    LoadPage();
                }
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
                var res = MessageBox.Show($"Are you sure to delete this order: {selectedOrder.OrderId} - {selectedOrder.CustomerName}?", "Delete order", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (res == MessageBoxResult.Yes)
                {
                    int orderId = selectedOrder.OrderId;
                    orderBus.DeleteOrder(orderId);
                    LoadPage();
                }
            }
            else
            {
                MessageBox.Show("Please select an order to delete.");
            }
        }

        private void DetailBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.SelectedItem != null)
            {
                OrderModel selectedOrder = (OrderModel)OrderDataGrid.SelectedItem;
                int orderId = selectedOrder.OrderId;

                OrderDetailWindow orderDetailWindow = new OrderDetailWindow(orderId);
                orderDetailWindow.ShowDialog();
                LoadPage();
            }
            else
            {
                MessageBox.Show("Please select an order to view details.");
            }
        }

        private void ReloadPage(object sender, MouseButtonEventArgs e)
        {
            FromDatePicker.SelectedDate = null;
            ToDatePicker.SelectedDate = null;
            orderBus.FromDate = null;
            orderBus.ToDate = null;
            LoadPage();
        }
    }


}
