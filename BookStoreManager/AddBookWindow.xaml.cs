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
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        public BookModel NewBook {  get; set; }
        public BindingList<CategoryModel> Categories { get; set; }
        public AddBookWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NewBook = new BookModel();
            Categories = ManageBook.GetCategories();
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

        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            NewBook.ClearBook();
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            NewBook.ClearBook();
        }
    }
}
