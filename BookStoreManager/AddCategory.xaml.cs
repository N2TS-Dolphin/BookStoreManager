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
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddCategory : Window
    {
        public CategoryModel _newCategory {  get; set; }
        CategoryDao _categoryDao = new CategoryDao();
        public AddCategory()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _newCategory = new CategoryModel();
            tbox.DataContext = _newCategory;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int insertedID = _categoryDao.InsertNewCategory((CategoryModel)_newCategory.Clone());
            _newCategory.CategoryID = insertedID;
            DialogResult = true;
        }
    }
}
