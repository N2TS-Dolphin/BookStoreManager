using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        public BookModel NewBook { get; set; }
        public BindingList<CategoryModel> AllCategories { get; set; }
        public BindingList<CategoryModel> Categories { get; set; }
        public BindingList<string> ImageName { get; set; }
        public AddBookWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NewBook = new BookModel();
            DataContext = NewBook;
            categoryLV.ItemsSource = NewBook.Category;

            AllCategories = BookManagerBus.GetCategories();
            Categories = new BindingList<CategoryModel>();
            Categories = AllCategories;
            addCategoryCB.ItemsSource = Categories;

            ImageName = BookManagerBus.GetImageName();
            imageNameCB.ItemsSource = ImageName;
            imageNameCB.SelectedItem = ImageName[0];
        }

        private void removeCategory_Click(object sender, RoutedEventArgs e)
        {
            var selected = categoryLV.SelectedItem as CategoryModel;
            if (selected == null)
            {
                MessageBox.Show("Bạn phải chọn 1 danh mục trước.");
                return;
            }
            if (NewBook.Category.Contains(selected))
            {
                Categories.Add(selected);
                NewBook.Category.Remove(selected);
            }
        }

        private void addCategory_Click(object sender, RoutedEventArgs e)
        {
            var selected = addCategoryCB.SelectedItem as CategoryModel;
            if(selected == null)
            {
                MessageBox.Show("Bạn phải chọn 1 danh mục trước.");
                return;
            }
            if (!NewBook.Category.Contains(selected))
            {
                NewBook.Category.Add(selected);
                Categories.Remove(selected);
            }
        }
        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            int id = BookManagerBus.AddNewBook(NewBook);
            if (id == -1)
            {
                MessageBox.Show("Thêm thấtt bại");
            }
            else
            {
                Categories = AllCategories;
                NewBook.ClearBook();
            }
            MessageBox.Show("Thêm sách thình công.");
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            Categories = AllCategories;
            NewBook.ClearBook();
            imageNameCB.SelectedItem = ImageName[0];
            MessageBox.Show("Đã tải lại trang");
        }

        private void imageNameCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = imageNameCB.SelectedItem as string;
            if(selected != null)
            {
                NewBook.Image = selected;
            }
        }

        private void quitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();    
        }
    }
}
