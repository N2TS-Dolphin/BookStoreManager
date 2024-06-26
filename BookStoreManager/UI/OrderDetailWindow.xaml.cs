﻿using BookStoreManager.Database;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookStoreManager.UI
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
            this.Language = XmlLanguage.GetLanguage("vi-VN");
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
                    MessageBox.Show("Sách này đã có trong Chi tiết Đơn hàng, vui lòng chỉnh sửa số lượng nếu cần.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBoxResult result = MessageBox.Show($"Bạn có chắc muốn xóa mục này: " +
                    $"{selectedOrderDetail.Book.BookID} - {selectedOrderDetail.Book.BookName} ?",
                    "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

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
                        MessageBox.Show("Sách này đã có trong Chi tiết Đơn hàng, vui lòng chỉnh sửa số lượng nếu cần.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void productDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Lấy mục đã chọn từ DataGrid
            OrderDetailModel selectedOrderDetail = (OrderDetailModel)productDataGrid.SelectedItem;

            // Kiểm tra nếu có mục được chọn
            if (selectedOrderDetail != null)
            {
                var screen = new AddBookOrderDetailWindow(orderId, selectedOrderDetail);
                var result = screen.ShowDialog();

                if (result == true)
                {
                    var existingOrderDetail = orderDetails.FirstOrDefault(detail => detail.Book.BookID == screen._OrderDetail.Book.BookID);
                    if (existingOrderDetail != null && existingOrderDetail != selectedOrderDetail)
                    {
                        MessageBox.Show("Sách này đã có trong Chi tiết Đơn hàng, vui lòng chỉnh sửa số lượng nếu cần.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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


        private void productDataGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Find the DataGridRow under the mouse
            var row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row != null)
            {
                // Select the item
                row.IsSelected = true;

                // Create a context menu
                ContextMenu contextMenu = new ContextMenu();

                // Add Update option
                MenuItem updateMenuItem = new MenuItem();
                updateMenuItem.Header = "Update";
                updateMenuItem.Click += UpdateMenuItem_Click;
                contextMenu.Items.Add(updateMenuItem);

                // Add Delete option
                MenuItem deleteMenuItem = new MenuItem();
                deleteMenuItem.Header = "Delete";
                deleteMenuItem.Click += DeleteMenuItem_Click;
                contextMenu.Items.Add(deleteMenuItem);

                // Show the context menu
                contextMenu.IsOpen = true;
            }
        }

        private void UpdateMenuItem_Click(object sender, RoutedEventArgs e)
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
                        MessageBox.Show("Sách này đã có trong Chi tiết Đơn hàng, vui lòng chỉnh sửa số lượng nếu cần.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrderDetail = (OrderDetailModel)productDataGrid.SelectedItem;

            if (selectedOrderDetail != null)
            {
                MessageBoxResult result = MessageBox.Show($"Bạn có chắc muốn xóa mục này: " +
                    $"{selectedOrderDetail.Book.BookID} - {selectedOrderDetail.Book.BookName} ?",
                    "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    orderDetails.Remove(selectedOrderDetail);
                    UpdateTotalPrice();
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
            DateTime newOrderDate = order.OrderDate;
            int newTotalPrice = order.Price;
            string newAddress = order.OrderAddress;

            orderDetailBus.UpdateOrder(orderId, newOrderDate, newTotalPrice, newAddress);
            orderDetailBus.SaveNewOrderItems(orderId, orderDetails);
            DialogResult = true;
        }

        private void CancelOrderDetailBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void updateCustomerInfo_Click(object sender, RoutedEventArgs e)
        {
            var selectedCustomer = order.Customer;

            // Check if a customer is selected
            if (selectedCustomer == null)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng.");
                return;
            }

            // Open the update customer window with the selected customer
            var updateCustomerWindow = new UpdateCustomerWindow(selectedCustomer);
            updateCustomerWindow.Closed += UpdateCustomerWindow_Closed;
            updateCustomerWindow.ShowDialog();
        }

    
        private void UpdateCustomerWindow_Closed(object sender, EventArgs e)
        {
            LoadOrderDetails();
        }

    }
}
