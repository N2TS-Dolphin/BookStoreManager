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

using BookStoreManager.Database;
using BookStoreManager.Support;
using static System.Net.Mime.MediaTypeNames;

namespace BookStoreManager
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        connectDB database = new connectDB();

        public Dashboard()
        {
            InitializeComponent();
            database.accounts = database.readAccount();
        }
    }
}
