using BookStoreManager.Database;
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
    /// Interaction logic for UpdateCategory.xaml
    /// </summary>
    public partial class UpdateCategory : Window
    {
        public CategoryModel _selectedCategory { get; set; }
        CategoryDao _categoryDao = new CategoryDao();
        public UpdateCategory(CategoryModel selectedCategory)
        {
            InitializeComponent();
            _selectedCategory = selectedCategory;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _selectedCategory;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //_categoryDao.UpdateACategory((CategoryModel)_selectedCategory.Clone());
            DialogResult = true;
        }
    }
}
