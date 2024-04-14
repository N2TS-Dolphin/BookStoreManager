using System;
using System.Collections.Generic;
using System.ComponentModel;
using BookStoreManager.Process;
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

namespace BookStoreManager.UI
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
            DisplayingBook = BookManagerBus.CopyBook((BookModel)SavedBook.Clone());
            DisplayingBook.Category = BookManagerBus.CopyCategoryList(SavedBook.Category);
            SaveUnuseCategories = BookManagerBus.GetUnuseCategory(SavedBook);
            UnuseCategories = BookManagerBus.CopyCategoryList(SaveUnuseCategories);
            DeleteCategories = new();
            InsertCategories = new();

            ImageName = BookManagerBus.GetImageName();
            imageNameCB.ItemsSource = ImageName;
            imageNameCB.SelectedItem = DisplayingBook.Image;

            DataContext = DisplayingBook;
            TitleTBL.DataContext = SavedBook;
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
            BookManagerBus.UpdateBook(DisplayingBook, DeleteCategories, InsertCategories);
            SavedBook = BookManagerBus.CopyBook((BookModel)DisplayingBook.Clone());
            SavedBook.Category = BookManagerBus.CopyCategoryList(DisplayingBook.Category);

            DeleteCategories.Clear();
            InsertCategories.Clear();
            SaveUnuseCategories = BookManagerBus.CopyCategoryList(UnuseCategories);
            MessageBox.Show("Chỉnh sửa thình công.");
        }
        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayingBook = BookManagerBus.CopyBook((BookModel)SavedBook.Clone());
            DisplayingBook.Category = BookManagerBus.CopyCategoryList(SavedBook.Category);

            DeleteCategories.Clear();
            InsertCategories.Clear();
            UnuseCategories = BookManagerBus.CopyCategoryList(SaveUnuseCategories);

            DataContext = DisplayingBook;
            imageNameCB.SelectedItem = DisplayingBook.Image;

            categoryLV.ItemsSource = DisplayingBook.Category;
            addCategoryCB.ItemsSource = UnuseCategories;
            
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

        private void quitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
