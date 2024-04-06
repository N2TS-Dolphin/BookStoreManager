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
    /// Interaction logic for UpdateBookWindow.xaml
    /// </summary>
    public partial class UpdateBookWindow : Window
    {
        public BookModel SavedBook{ get; set; }
        public BookModel DisplayingBook { get; set; }
        public BindingList<CategoryModel> SaveUnuseCategories { get; set; }
        public BindingList<CategoryModel> UnuseCategories { get; set; }
        public BindingList<CategoryModel> InsertCategories { get; set; }
        public BindingList<CategoryModel> DeleteCategories { get; set; }
        public BindingList<string> ImageName { get; set; }
        public UpdateBookWindow(BookModel SelectedBook)
        {
            InitializeComponent();
            SavedBook = (BookModel)SelectedBook.Clone();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayingBook = (BookModel)SavedBook.Clone();
            SaveUnuseCategories = ManageBook.GetUnuseCategory(SavedBook);
            UnuseCategories = SaveUnuseCategories;
            DeleteCategories = new();
            InsertCategories = new();

            ImageName = ManageBook.GetImageName();
            imageNameCB.ItemsSource = ImageName;
            imageNameCB.SelectedItem = DisplayingBook.Image;

            DataContext = DisplayingBook;
            TitleTBL.DataContext = SavedBook.BookID;
            categoryLV.ItemsSource = DisplayingBook.Category;
            addCategoryCB.ItemsSource = UnuseCategories;
        }

        private void removeCategory_Click(object sender, RoutedEventArgs e)
        {
            var selected = categoryLV.SelectedItem as CategoryModel;
            if (selected == null)
            {
                MessageBox.Show("Bạn phải chọn 1 danh mục trước.");
                return;
            }
            if (DisplayingBook.Category.Contains(selected))
            {
                if (InsertCategories.Contains(selected))
                {
                    InsertCategories.Remove(selected);
                }
                else
                {
                    DeleteCategories.Add(selected);
                }
                UnuseCategories.Add(selected);
                DisplayingBook.Category.Remove(selected);
            }
        }

        private void addCategory_Click(object sender, RoutedEventArgs e)
        {
            var selected = addCategoryCB.SelectedItem as CategoryModel;
            if (selected == null)
            {
                MessageBox.Show("Bạn phải chọn 1 danh mục trước.");
                return;
            }
            if (UnuseCategories.Contains(selected))
            {
                if (DeleteCategories.Contains(selected))
                {
                    DeleteCategories.Remove(selected);
                }
                else
                {
                    InsertCategories.Add(selected);
                }
                DisplayingBook.Category.Add(selected);
                UnuseCategories.Remove(selected);
            }
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            ManageBook.UpdateBook(DisplayingBook, DeleteCategories, InsertCategories);
            SavedBook = DisplayingBook;
            DeleteCategories.Clear();
            InsertCategories.Clear();
            SaveUnuseCategories = UnuseCategories;
            MessageBox.Show("Chỉnh sửa thình công.");
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayingBook = (BookModel)SavedBook.Clone();
            imageNameCB.SelectedItem = DisplayingBook.Image;
            DeleteCategories.Clear();
            InsertCategories.Clear();
            UnuseCategories = SaveUnuseCategories;
            MessageBox.Show("Đã tải lại trang");
        }

        private void imageNameCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = imageNameCB.SelectedItem as string;
            if (selected != null)
            {
                DisplayingBook.Image = selected;
            }
        }
    }
}
