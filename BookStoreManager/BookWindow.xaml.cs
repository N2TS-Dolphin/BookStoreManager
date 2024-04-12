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
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;

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
        public BindingList<string> SortName { get; set; }
        public BookWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BookShell = new BookShellBus();
            CategoryList = BookShell.GetAllCategory();
            categoryListView.ItemsSource = CategoryList;
            
            itemCountTB.Text = "9";

            SortName = BookShell.GetSortName();
            sortCB.ItemsSource = SortName;
            sortCB.SelectedIndex = 0;

            LoadBookList();
            LoadBookDetail(0);            
        }
        public void LoadBookList()
        {
            var (items, totalPages, currentPage) = BookShell.GetBookList();
            BookList = items;
            bookListView.ItemsSource = BookList;

            txtItemPage.Text = $"{currentPage}/{totalPages}";
        }
        public void LoadBookDetail(int index)
        {
            if (BookList.Count == 0)
            {
                if (BookDetail != null) { BookDetail.ClearBook(); }
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
                    try
                    {
                        BookList = BookShell.DeleteBook(selectedBook, BookList);
                        MessageBox.Show($"Xóa quyển sách {name} thành công.");
                        LoadBookList();
                        LoadBookDetail(0);
                    }
                    catch(Exception ex) { MessageBox.Show($"Xóa quyển sách {name} thất bại."); }
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
                try
                {
                    var newCategory = addCategory._newCategory;
                    CategoryList = BookShell.AddCategory(newCategory, CategoryList);
                    MessageBox.Show($"Thêm danh mục {newCategory.CategoryName} thành công.");

                    Refresh();
                }
                catch (Exception ex) { MessageBox.Show($"Thêm danh mục thất bại."); }
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
                    try
                    {
                        var newCategory = updateCategory._selectedCategory;
                        CategoryList = BookShell.UpdateCategory(selectedIndex, newCategory, CategoryList);
                        MessageBox.Show($"Chỉnh sửa danh mục {newCategory.CategoryName} thành công.");

                        Refresh();
                    }
                    catch (Exception ex) { MessageBox.Show($"Chỉnh sửa danh mục {CategoryList[selectedIndex].CategoryName} thất bại."); }
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
                    try
                    {
                        CategoryList = BookShell.DeleteCategory(selectedIndex, CategoryList);
                        MessageBox.Show($"Xóa danh mục {name} thành công.");

                        Refresh();
                    }
                    catch (Exception ex) { MessageBox.Show($"Xóa danh mục {name} thất bại."); }
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
            var priceFromText = priceFromTB.Text;
            var priceToText = priceToTB.Text;
            int priceFrom, priceTo;
            if (int.TryParse(priceFromText, out priceFrom) && int.TryParse(priceToText, out priceTo))
            {
                if(priceFrom >= 0 && priceTo >= priceFrom) { 
                    BookShell.FilterPrice(priceFrom, priceTo);
                    LoadBookList();
                    LoadBookDetail(0);
                }
                else{ MessageBox.Show("Giá đầu phải nhỏ hơn giá cuối"); }
            }
            else { MessageBox.Show("Giá tiền đang lọc không phải số. Vui lòng nhập số hợp lệ."); }
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
            MessageBox.Show("Đã tải lại trang");
        }

        private void execlButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                BookShell.ImportFromExcel(filePath, CategoryList);
                MessageBox.Show($"Tải dữ liệu thành công.");
                Refresh();
            }
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
        private void itemCountButton_Click(object sender, RoutedEventArgs e)
        {
            var itemPerPageText = itemCountTB.Text;
            int itemPerPage;
            if (int.TryParse(itemPerPageText, out itemPerPage))
            {
                BookShell.ChangeItemPerPage(itemPerPage);
                LoadBookList();
                LoadBookDetail(0);
            }
            else { MessageBox.Show("Số sản phẩm mỗi phải là số. Vui lòng nhập lại"); }
        }
        private void sortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sort = sortCB.SelectedIndex;
            BookShell.SortBook(sort);

            LoadBookList();
            LoadBookDetail(0);
        }
    }
}
