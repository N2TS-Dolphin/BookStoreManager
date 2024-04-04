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
        //BookDao BookDao;
        //CategoryDao CategoryDao;
        int _currentPage = 1, _totalPages = 0; 
        string _search = "", _category = "";
        public BookWindow()
        {
            InitializeComponent();
            //BookDao = new BookDao();
            //CategoryDao = new CategoryDao();
            new BookDao();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _categoryList = CategoryDao.getCategoryList();
            _categoryList.Insert(0, new CategoryModel(0, "All"));
            categoryListView.ItemsSource = _categoryList;
            loadBookList();
            loadBookDetail(0);
        }
        public void loadBookList()
        {
            var (items, totalItems, totalPages) = BookDao.getBookList(_currentPage, 10, _search, _category);
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
            _bookDetail = BookDao.getBookDetail(selectedID);
            _bookDetail.Category = CategoryDao.getBookCategory(selectedID);
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
                try
                {
                    var newCategory = addCategory._newCategory;
                    int insertedID = CategoryDao.InsertNewCategory((CategoryModel)newCategory.Clone());
                    newCategory.CategoryID = insertedID;
                    _categoryList.Add(newCategory);
                    MessageBox.Show($"Thêm danh mục {newCategory.CategoryName} thành công.");
                }
                catch (Exception ex) { MessageBox.Show("Thêm danh mục không thành công."); }
            }
        }
        private void updateCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = categoryListView.SelectedIndex;
            if(selectedIndex > 0 && selectedIndex < _categoryList.Count)
            {
                var id = _categoryList[selectedIndex].CategoryID;
                UpdateCategory updateCategory = new UpdateCategory((CategoryModel)_categoryList[selectedIndex].Clone());
                updateCategory.ShowDialog();
                if (updateCategory.DialogResult == true)
                {
                    try
                    {
                        var newCategory = updateCategory._selectedCategory;
                        CategoryDao.UpdateACategory((CategoryModel)newCategory.Clone());
                        _categoryList[selectedIndex].CategoryName = newCategory.CategoryName;
                        MessageBox.Show($"Chỉnh sửa danh mục {newCategory.CategoryName} thành công.");
                    }
                    catch (Exception ex) { MessageBox.Show($"Chỉnh sửa danh mục {_categoryList[selectedIndex].CategoryName} không thành công."); }
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
                        CategoryDao.DeleteACategory(id);
                        _categoryList.RemoveAt(selectedIndex);
                        MessageBox.Show($"Xóa danh mục {name} thành công.");
                    }
                    catch (Exception ex) { MessageBox.Show($"Xóa danh mục {name} không thành công."); }
                }
            }
            else { MessageBox.Show("Chọn danh mục muốn xóa"); }
        }
        private void categoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = categoryListView.SelectedIndex;
            selected = (selected > 0 && selected < _categoryList.Count) ? selected : 0;
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
