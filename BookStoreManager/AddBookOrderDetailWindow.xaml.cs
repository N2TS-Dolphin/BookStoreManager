﻿using BookStoreManager.Database;
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

namespace BookStoreManager
{
    /// <summary>
    /// Interaction logic for AddBookOrderDetail.xaml
    /// </summary>
    public partial class AddBookOrderDetailWindow : Window
    {
        private BindingList<CategoryModel> _categories;
        private BindingList<BookModel> _books;
        private BookModel _selectedBook;
        public OrderDetailModel _OrderDetail;
        private int _orderId;
        private OrderDetailBus _orderDetailBus = new OrderDetailBus(); // Create an instance of OrderDetailBus

        public AddBookOrderDetailWindow(int orderId)
        {
            InitializeComponent();
            _orderId = orderId;
        }

        public AddBookOrderDetailWindow(int orderId, OrderDetailModel OrderDetail)
        {
            InitializeComponent();
            _orderId = orderId;
            this._OrderDetail = OrderDetail;
            this.DataContext = _OrderDetail;
            _selectedBook = OrderDetail.Book;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            _categories = _orderDetailBus.GetAllCategory(); // Use OrderDetailBus to get all categories
            categoryCombobox.ItemsSource = _categories;
            categoryCombobox.SelectedIndex = 0;
            LoadBooksByCategory(_categories[categoryCombobox.SelectedIndex].CategoryID);
        }

        private void LoadBooksByCategory(int categoryId)
        {
            _books = _orderDetailBus.GetBooksByCategory(categoryId); // Use OrderDetailBus to get books by category
            BookListView.ItemsSource = _books;
        }

        private void BookListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedBook = (BookModel)BookListView.SelectedItem;
            if (_selectedBook != null)
            {
                // Display the selected book details
                BookTextBox.Text = _selectedBook.BookName;
                QuantityTextBox.Text = "0"; // Set default quantity
            }
        }

        private void categoryCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (categoryCombobox.SelectedIndex >= 0)
            {
                LoadBooksByCategory(_categories[categoryCombobox.SelectedIndex].CategoryID);
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedBook != null)
            {
                // Validate the quantity input
                if (int.TryParse(QuantityTextBox.Text, out int quantity))
                {
                    if (quantity > 0)
                    {
                        // Create a new OrderDetailModel with the selected book and quantity
                        _OrderDetail = new OrderDetailModel
                        {
                            OrderID = _orderId,
                            Book = _selectedBook,
                            Quantity = quantity
                        };

                        // Use OrderDetailBus to insert the order detail
                        _orderDetailBus.InsertOrderItem(_orderId,  _OrderDetail );

                        // Close the window and set DialogResult to true
                        DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show("Quantity must be greater than 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid quantity. Please enter a valid number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a book.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            // Close the window without making any changes
            DialogResult = false;
        }


    }
}