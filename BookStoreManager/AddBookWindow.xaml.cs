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
        //BookModel NewBook = new BookModel();
        public BindingList<CategoryModel> AllCategories { get; set; }
        public BindingList<CategoryModel> Categories { get; set; }
        public AddBookWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NewBook = new BookModel();
            //AllCategories = ManageBook.GetCategories();
            //Categories = new BindingList<CategoryModel>();
            //Categories = AllCategories;
            //NewBook.Image = "/Image/tempID_BookIMG.jpg";
            //Categories = ManageBook.GetCategories();
            categoryLV.ItemsSource = NewBook.Category;
            addCategoryCB.ItemsSource = Categories;
            DataContext = NewBook;
        }

        private void removeCategory_Click(object sender, RoutedEventArgs e)
        {
            var selected = categoryLV.SelectedItem as CategoryModel;
            if (selected == null)
            {
                MessageBox.Show("Choose item first");
                return;
            }
            if (NewBook.Category.Contains(selected))
            {
                try
                {
                    Categories.Add(selected);
                    NewBook.Category.Remove(selected);
                    MessageBox.Show("Success");
                }
                catch (Exception) { MessageBox.Show("Failed"); }

            }
        }

        private void addCategory_Click(object sender, RoutedEventArgs e)
        {
            var selected = addCategoryCB.SelectedItem as CategoryModel;
            if(selected == null)
            {
                MessageBox.Show("Choose item first");
                return;
            }
            if (!NewBook.Category.Contains(selected))
            {
                try
                {
                    NewBook.Category.Add(selected);
                    Categories.Remove(selected);
                    MessageBox.Show("Success");
                }catch(Exception) { MessageBox.Show("Failed"); }
            }
        }

        private void changeIMG_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Image files (.png;.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (.)|*.*";

            //if (openFileDialog.ShowDialog() == true)
            //{
            //    string filePath = openFileDialog.FileName;
            //    MessageBox.Show($"{filePath}");
            //    if (!String.IsNullOrEmpty(NewBook.Image))
            //    {
            //        if (!ManageBook.DeleteImageFromProject(NewBook.Image))
            //        {
            //            return;
            //        }
            //    }
            //    var newFileName = ManageBook.SaveImageToProject(filePath, "tempID");
            //    NewBook.Image = $"/Image/{newFileName}";

            //    //var uri = new Uri($"/Image/{newFileName}", UriKind.Relative);
            //    //var bitmap = new BitmapImage(uri);
            //    //bookImg.Source = bitmap;
            //    MessageBox.Show(NewBook.Image);
            //}
            string Img = ImageTB.Text;
            NewBook.Image = Img;
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            int id = ManageBook.AddNewBook(NewBook);
            if (id == -1)
            {
                MessageBox.Show("Them that bai");
            }
            else
            {
                MessageBox.Show($"{id}");
                NewBook.ClearBook();
            }
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            NewBook.ClearBook();
        }
    }
}
