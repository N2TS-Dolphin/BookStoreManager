using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;

using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;
using MySqlX.XDevAPI.Common;
using BookStoreManager.Database;
using BookStoreManager.Support;

namespace BookStoreManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<accountInfo> accounts = new List<accountInfo>();
        login accountLogged = new login();
        readDB read = new readDB();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            accounts = read.LoadDataFromDatabase();
            int index = accounts.FindIndex(x => x.username == txtUsername.Text);

            if (index != -1)
            {
                if (accounts[index].password == txtPassword.Password)
                {
                    accountLogged.index = index;
                    read.accounts = accounts;
                    Dashboard window = new Dashboard();
                    this.Close();
                    window.Show();
                }
                else
                {
                    MessageBox.Show("Username hoặc Password không đúng");
                }
            }
            else
            {
                MessageBox.Show("Username hoặc Password không đúng");
            }

        }
        
    }
}