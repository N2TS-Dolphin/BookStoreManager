using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
using BookStoreManager.Database;

namespace BookStoreManager
{
    /// <summary>
    /// Interaction logic for BookWindow.xaml
    /// </summary>
    public partial class BookWindow : Window
    {
        BindingList<BookModel> _bookList = new();
        BindingList<CategoryModel> _categoryList = new();
        BookModel _bookDetail = new();
        BookDao _bookDao;
        CategoryDao _categoryDao;
        int _currentPage = 1, _totalPages = 0; 
        string _search = "", _category = "";
        public BookWindow()
        {
            InitializeComponent();
            _bookDao = new BookDao();
            _categoryDao = new CategoryDao();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _categoryList = _categoryDao.getCategoryList();
            _categoryList.Insert(0, new CategoryModel(0, "All"));
            categoryListView.ItemsSource = _categoryList;
            loadBookList();
            loadBookDetail(0);
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
        public void loadBookDetail(int index)
        {
            if(_bookList.Count == 0)
            {
                _bookDetail.clearBook();
                return;
            }
            int selectedID = _bookList[index].BookID;
            _bookDetail = _bookDao.getBookDetail(selectedID);
            _bookDetail.Category = _categoryDao.getBookCategory(selectedID);
            bookDetail.DataContext = _bookDetail;
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

        private void bookListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedIndex = bookListView.SelectedIndex; 
            if (selectedIndex < 0) return;
            loadBookDetail(selectedIndex);
        }

        private void updateBookButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deteleBookButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void addCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddCategory addCategory = new AddCategory();
            addCategory.ShowDialog();
            if(addCategory.DialogResult == true)
            {
                var newCategory = addCategory._newCategory;
                _categoryList.Add(newCategory);
            }
        }
        private void updateCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = categoryListView.SelectedIndex;
            if(selectedIndex > 0 && selectedIndex < _categoryList.Count)
            {
                UpdateCategory updateCategory = new UpdateCategory((CategoryModel)_categoryList[selectedIndex].Clone());
                updateCategory.ShowDialog();
                if (updateCategory.DialogResult == true)
                {
                    var newCategory = updateCategory._selectedCategory;
                    _categoryList[selectedIndex] = newCategory;
                }
            }
            else { MessageBox.Show("Chọn danh mục trước khi sửa"); }
        }
        private void deteleCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = categoryListView.SelectedIndex;
            if (selectedIndex > 0 && selectedIndex < _categoryList.Count)
            {
                var id = _categoryList[selectedIndex].CategoryID;
                var name = _categoryList[selectedIndex].CategoryName;
                var messbox = MessageBox.Show($"Bạn có chắc muốn xóa danh mục {name}, id = {id}?", $"Xóa danh mục {name}"
                    , MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if(messbox == MessageBoxResult.OK) {
                    try
                    {
                        _categoryDao.DeleteACategory(id);
                        _categoryList.RemoveAt(selectedIndex);
                        MessageBox.Show("Delete Success");
                    }
                    catch (Exception ex) { MessageBox.Show("Delete Failed"); }
                }
            }
            else { MessageBox.Show("Chọn danh mục muốn xóa"); }
        }
        private void categoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = categoryListView.SelectedIndex;
            selected = (selected > 0 && selected < _categoryList.Count) ? selected : 0;
            MessageBox.Show($"{selected}");
            _category = (selected == 0) ? "" : _categoryList[selected].CategoryName;
            _currentPage = 1;
            loadBookList();
            loadBookDetail(0);
        }

        private void searchButton_click(object sender, RoutedEventArgs e)
        {
            _search = tboxSearch.Text;
            _currentPage = 1;
            loadBookList();
            loadBookDetail(0);
        }
    }
}
