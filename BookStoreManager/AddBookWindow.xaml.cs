using System;
using System.Collections.Generic;
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
        public AddBookWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NewBook = new BookModel();
            DataContext = NewBook;
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
            NewBook = new BookModel();
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            NewBook = new BookModel();
        }

    }
}
