﻿using BookStoreManager.Database;
using BookStoreManager.DataType;
using BookStoreManager.Process;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace BookStoreManager.UI
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
            var (items, totalItems, totalPages, current) = orderBus.GetAllPaging(FromDatePicker.SelectedDate, ToDatePicker.SelectedDate);
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
                else
                {
                    LoadPage();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn đơn hàng để cập nhật.");
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.SelectedItem != null)
            {
                OrderModel selectedOrder = (OrderModel)OrderDataGrid.SelectedItem;
                var res = MessageBox.Show($"Bạn có chắc muốn xóa đơn hàng: {selectedOrder.OrderId} - {selectedOrder.Customer.CustomerName}?", "Xóa đơn hàng", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (res == MessageBoxResult.Yes)
                {
                    int orderId = selectedOrder.OrderId;
                    orderBus.DeleteOrder(orderId);
                    LoadPage();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn đơn hàng để xóa.");
            }
        }

        private void OrderDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Get the selected item from the DataGrid
            OrderModel selectedOrder = (OrderModel)OrderDataGrid.SelectedItem;

            // Check if an item is selected
            if (selectedOrder != null)
            {
                int orderId = selectedOrder.OrderId;

                // Open the OrderDetailWindow for updating
                OrderDetailWindow orderDetailWindow = new OrderDetailWindow(orderId);
                var result = orderDetailWindow.ShowDialog();

                // Reload the page if the window was closed successfully
                if (result == true)
                {
                    LoadPage();
                }
            }
        }

        private void OrderDataGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
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
                MessageBox.Show("Vui lòng chọn đơn hàng để xem chi tiết.");
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.SelectedItem != null)
            {
                OrderModel selectedOrder = (OrderModel)OrderDataGrid.SelectedItem;
                var res = MessageBox.Show($"Bạn có chắc muốn xóa đơn hàng: {selectedOrder.OrderId} - {selectedOrder.Customer.CustomerName}?", "Xóa đơn hàng", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (res == MessageBoxResult.Yes)
                {
                    int orderId = selectedOrder.OrderId;
                    orderBus.DeleteOrder(orderId);
                    LoadPage();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn đơn hàng để xóa.");
            }
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            FromDatePicker.SelectedDate = null;
            ToDatePicker.SelectedDate = null;
            LoadPage();
            MessageBox.Show("Tải lại trang thành công");
        }
    }


}
