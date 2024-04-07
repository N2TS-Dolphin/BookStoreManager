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
        //int _currentPage = 1, _totalPages = 0;
        //string _search = "", _category = "";
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
            var (items, totalItems, totalPages, currentPage) = BookShell.GetBookList();
            BookList = items;
            bookListView.ItemsSource = BookList;

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
            BookDetail = (BookModel)BookList[index].Clone();
            BookDetail.Category = BookShell.GetBookCategory((BookModel)BookDetail.Clone());
            bookDetail.DataContext = BookDetail;
            lvCategory.ItemsSource = BookDetail.Category;
        }
        public void Refresh()
        {
            BookShell.RefreshPage(CategoryList);
            LoadBookList();
            LoadBookDetail(0);
        }
        private void prevButton_click(object sender, RoutedEventArgs e)
        {

            BookShell.MoveToPreviousPage();
            LoadBookList();
        }

        private void nextButton_click(object sender, RoutedEventArgs e)
        {
            BookShell.MoveToNextPage();
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
            updateBookWindow.Closed += ManageBookWindow_Closed;
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
                    BookList = BookShell.DeleteBook(selectedBook, BookList);
                    MessageBox.Show($"Xóa quyển sách {name} thành công.");
                    Refresh();
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
                CategoryList = BookShell.AddCategory(newCategory, CategoryList);
                MessageBox.Show($"Thêm danh mục {newCategory.CategoryName} thành công.");

                Refresh();
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
                    CategoryList = BookShell.UpdateCategory(selectedIndex, newCategory, CategoryList);
                    MessageBox.Show($"Chỉnh sửa danh mục {newCategory.CategoryName} thành công.");

                    Refresh();
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
                    CategoryList = BookShell.DeleteCategory(selectedIndex, CategoryList);
                    MessageBox.Show($"Xóa danh mục {name} thành công.");

                    Refresh();
                }
            }
            else { MessageBox.Show("Chọn danh mục muốn xóa"); }
        }
        private void categoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = categoryListView.SelectedIndex;
            BookShell.ChangeSelectionCategory(selected, CategoryList);
            LoadBookList();
            LoadBookDetail(0);
        }

        private void searchButton_click(object sender, RoutedEventArgs e)
        {
            var search = tboxSearch.Text;
            BookShell.SearchBook(search);
            LoadBookList();
            LoadBookDetail(0);
        }

        private void priceButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
            MessageBox.Show("Đã tải lại trang");
        }

        private void execlButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow addBookWindow = new AddBookWindow();
            addBookWindow.Closed += ManageBookWindow_Closed;
            addBookWindow.Show();
        }
        private void ManageBookWindow_Closed(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
