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
    /// Interaction logic for CreateNewAccount.xaml
    /// </summary>
    public partial class CreateNewAccount : Window
    {
        connectDB database = new connectDB();
        public CreateNewAccount()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            database.writeAccount(txtUsername.Text, txtPassword.Password, txtFullname.Text);
            this.Close();
        }
    }
}
