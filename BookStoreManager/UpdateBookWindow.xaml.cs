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
        public BindingList<CategoryModel> AllCategories { get; set; }
        public BindingList<CategoryModel> UnuseCategories { get; set; }
        public BindingList<CategoryModel> AddedCategories { get; set; }
        public BindingList<CategoryModel> DeleteCategories { get; set; }
        public UpdateBookWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void removeCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void changeIMG_Click(object sender, RoutedEventArgs e)
        {

        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
