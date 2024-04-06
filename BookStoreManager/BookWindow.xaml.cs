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
        public BindingList<BookModel> BookList { get; set; }
        public BindingList<CategoryModel> CategoryList { get; set; }
        public BookModel BookDetail { get; set; }
        public BookShellBus BookShell { get; set; }
        int _currentPage = 1, _totalPages = 0;
        string _search = "", _category = "";
        public BookWindow()
        {
            InitializeComponent();
            new BookDao();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BookShell = new BookShellBus();
            CategoryList = BookShell.GetAllCategory();
            categoryListView.ItemsSource = CategoryList;
            LoadBookList();
            LoadBookDetail(0);
        }
        public void LoadBookList()
        {
            var (items, totalItems, totalPages, currentPage) = BookShell.LoadBookList();
            BookList = items;
            bookListView.ItemsSource = BookList;
            //_totalPages = totalPages;
            //var currentPage = (totalPages == 0) ? 0 : _currentPage;
            txtItemsCount.Text = $"Kết quả: {totalItems}";
            txtItemPage.Text = $"{currentPage}/{totalPages}";
        }
        public void LoadBookDetail(int index)
        {
            if (BookList.Count == 0)
            {
                BookDetail.ClearBook();
                return;
            }
            int selectedID = BookList[index].BookID;
            BookDetail = (BookModel)BookList[index].Clone();
            BookDetail.Category = CategoryDao.GetBookCategoryFromDB(selectedID);
            bookDetail.DataContext = BookDetail;
            lvCategory.ItemsSource = BookDetail.Category;
        }
        public void refreshPage()
        {
            _category = CategoryList[0].CategoryName;
            _search = "";
            _currentPage = 1;
            LoadBookList();
            LoadBookDetail(0);
        }
        private void prevButton_click(object sender, RoutedEventArgs e)
        {
            _currentPage = (_currentPage-- <= 1) ? 1 : _currentPage;
            LoadBookList();
        }

        private void nextButton_click(object sender, RoutedEventArgs e)
        {
            _currentPage = (_currentPage++ >= _totalPages) ? _totalPages : _currentPage;
            LoadBookList();
        }

        private void bookListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedIndex = bookListView.SelectedIndex;
            if (selectedIndex < 0) return;
            LoadBookDetail(selectedIndex);
        }

        private void updateBookButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateBookWindow updateBookWindow = new UpdateBookWindow((BookModel)BookDetail.Clone());
            updateBookWindow.Show();    
        }

        private void deteleBookButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = bookListView.SelectedIndex;
            if (selectedIndex > 0 && selectedIndex < BookList.Count)
            {
                var selectedBook = BookList[selectedIndex];
                var id = selectedBook.BookID;
                var name = selectedBook.BookName;
                var messbox = MessageBox.Show($"Bạn có chắc muốn xóa quyển sách {name}, id = {id}?", $"Xóa quyển sách {name}"
                    , MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (messbox == MessageBoxResult.OK)
                {
                    CategoryDao.DeleteAllBookCategoryFromDB(selectedBook);
                    BookDao.DeleteBookFromDB(selectedBook);
                    BookList.Remove(selectedBook);
                    //CategoryDao.DeleteACategoryFromDB(id);
                    //CategoryList.RemoveAt(selectedIndex);
                    MessageBox.Show($"Xóa quyển sách {name} thành công.");
                }
            }
            else { MessageBox.Show("Chọn quyển sách muốn xóa"); }
        }
        private void addCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddCategory addCategory = new AddCategory();
            addCategory.ShowDialog();
            if (addCategory.DialogResult == true)
            {
                var newCategory = addCategory._newCategory;
                int insertedID = CategoryDao.InsertNewCategoryToDB((CategoryModel)newCategory.Clone());
                newCategory.CategoryID = insertedID;
                CategoryList.Add(newCategory);
                CategoryList.OrderBy(x => x.CategoryName).ToList();
                MessageBox.Show($"Thêm danh mục {newCategory.CategoryName} thành công.");
            }
        }
        private void updateCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = categoryListView.SelectedIndex;
            if (selectedIndex > 0 && selectedIndex < CategoryList.Count)
            {
                var id = CategoryList[selectedIndex].CategoryID;
                UpdateCategory updateCategory = new UpdateCategory((CategoryModel)CategoryList[selectedIndex].Clone());
                updateCategory.ShowDialog();
                if (updateCategory.DialogResult == true)
                {
                    var newCategory = updateCategory._selectedCategory;
                    CategoryDao.UpdateACategoryToDB((CategoryModel)newCategory.Clone());
                    CategoryList[selectedIndex].CategoryName = newCategory.CategoryName;
                    MessageBox.Show($"Chỉnh sửa danh mục {newCategory.CategoryName} thành công.");
                } 
            }
            else { MessageBox.Show("Chọn danh mục trước khi sửa"); }
        }
        private void deteleCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = categoryListView.SelectedIndex;
            if (selectedIndex > 0 && selectedIndex < CategoryList.Count)
            {
                var id = CategoryList[selectedIndex].CategoryID;
                var name = CategoryList[selectedIndex].CategoryName;
                var messbox = MessageBox.Show($"Bạn có chắc muốn xóa danh mục {name}, id = {id}?", $"Xóa danh mục {name}"
                    , MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (messbox == MessageBoxResult.OK)
                {
                    CategoryDao.DeleteACategoryFromDB(id);
                    CategoryList.RemoveAt(selectedIndex);
                    MessageBox.Show($"Xóa danh mục {name} thành công.");
                }
            }
            else { MessageBox.Show("Chọn danh mục muốn xóa"); }
        }
        private void categoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = categoryListView.SelectedIndex;
            selected = (selected > 0 && selected < CategoryList.Count) ? selected : 0;
            _category = (selected == 0) ? "" : CategoryList[selected].CategoryName;
            _currentPage = 1;
            LoadBookList();
            LoadBookDetail(0);
        }

        private void searchButton_click(object sender, RoutedEventArgs e)
        {
            _search = tboxSearch.Text;
            _currentPage = 1;
            LoadBookList();
            LoadBookDetail(0);
        }

        private void priceButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            refreshPage();
            MessageBox.Show("Đã tải lại trang");
        }

        private void execlButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow addBookWindow = new AddBookWindow();
            addBookWindow.Show();
        }
    }
}
