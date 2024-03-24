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
    /// Interaction logic for BookWindow.xaml
    /// </summary>
    public partial class BookWindow : Window
    {
        BindingList<BookModel> _bookList = new();
        BindingList<CategoryModel> _categoryList = new();
        BookDao _bookDao = new BookDao();
        CategoryDao _categoryDao = new CategoryDao();
        int _currentPage = 1, _totalPages = 0; 
        string _search = "", _category = "";
        public BookWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _categoryList = _categoryDao.getCategoryList();
            _categoryList.Insert(0, new CategoryModel("000", "All"));
            categoryListView.ItemsSource = _categoryList;
            loadBookList();
        }
        public void loadBookList()
        {
            var (items, totalItems, totalPages) = _bookDao.getBookList(_currentPage, 10, _search, _category);
            _bookList = items;
            bookListView.ItemsSource = _bookList;
            _totalPages = totalPages;
            var currentPage = (totalPages == 0) ? 0 : _currentPage;
            txtItemsCount.Text = $"Item count: {totalItems}";
            txtItemPage.Text = $"{currentPage}/{totalPages}";
        }

        private void prevButton_click(object sender, RoutedEventArgs e)
        {
            _currentPage = (_currentPage-- <= 1) ? 1 : _currentPage;
            loadBookList();
        }

        private void nextButton_click(object sender, RoutedEventArgs e)
        {
            _currentPage = (_currentPage++ >= _totalPages) ? _totalPages : _currentPage;
            loadBookList();
        }

        private void categoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = categoryListView.SelectedIndex;
            _category = (selected == 0) ? "" : _categoryList[selected].CategoryName;
            _currentPage = 1;
            loadBookList();
        }

        private void searchButton_click(object sender, RoutedEventArgs e)
        {
            _search = tboxSearch.Text;
            _currentPage = 1;
            loadBookList();
        }
    }
}
