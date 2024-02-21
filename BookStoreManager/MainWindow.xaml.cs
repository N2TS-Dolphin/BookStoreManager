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

namespace BookStoreManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            List<accountInfo> accounts = LoadDataFromDatabase();
            int index = accounts.FindIndex(x => x.username == txtUsername.Text);

            if (index != -1)
            {
                if (accounts[index].password == txtPassword.Password)
                {
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

        private List<accountInfo> LoadDataFromDatabase()
        {
            List<accountInfo> accounts = new List<accountInfo>();

            using (MySqlConnection connection = DBUtils.GetDBConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT * FROM datatest.account", connection);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string user = reader.GetString("username");
                            string pass = reader.GetString("password");

                            accountInfo temp = new accountInfo()
                            {
                                username = user,
                                password = pass
                            };

                            accounts.Add(temp);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            return accounts;
        }
    }
}