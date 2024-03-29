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
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Configuration;
using Microsoft.VisualBasic.ApplicationServices;
using System.Data.Common;

using BookStoreManager.DataType;
using BookStoreManager.Database;

namespace BookStoreManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AccountDao account = new AccountDao();
        public MainWindow()
        {
            InitializeComponent();
            savetoConfig("admin", "admin");
            account.readAccount();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var result = loadfromConfig();
            txtUsername.Text = result.Item1;
            txtPassword.Password = result.Item2;
        }

        /// <summary>
        /// Nút đăng nhập
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            int index = account.accounts.FindIndex(x => x.username == txtUsername.Text);
            
            if (index != -1)
            {
                var passwordInByte = Convert.FromBase64String(account.accounts[index].password);
                var entropyInByte = Convert.FromBase64String(account.accounts[index].entropy);
                var decryptedPassword = ProtectedData.Unprotect(passwordInByte, entropyInByte, DataProtectionScope.CurrentUser);

                var password = Encoding.UTF8.GetString(decryptedPassword);
                if (password == txtPassword.Password)
                {
                    Login.Instance.Set(index);
                    if(cbxRemember.IsChecked == true)
                    {
                        savetoConfig(txtUsername.Text, txtPassword.Password);
                    }
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

        /// <summary>
        /// Lưu thông tin đăng nhập gần nhất vào App.config
        /// </summary>
        /// <param name="user">username người dùng nhập</param>
        /// <param name="pass">password người dùng nhập</param>
        public void savetoConfig(string user, string pass)
        {
            var password = pass;

            var passwordInByte = Encoding.UTF8.GetBytes(password);
            var entropy = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(entropy);
            }

            var cypherText = ProtectedData.Protect(passwordInByte, entropy, DataProtectionScope.CurrentUser);

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["username"].Value = user;
            config.AppSettings.Settings["password"].Value = Convert.ToBase64String(cypherText);
            config.AppSettings.Settings["entropy"].Value = Convert.ToBase64String(entropy);

            config.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// Tải thông tin đăng nhập gần nhất từ App.config
        /// </summary>
        /// <returns>Trả về username và password</returns>
        public (string, string) loadfromConfig()
        {
            var username = ConfigurationManager.AppSettings["username"];
            var passwordInConfig = ConfigurationManager.AppSettings["password"];
            var entropyInConfig = ConfigurationManager.AppSettings["entropy"];

            if (username != "")
            {
                var passwordInByte = Convert.FromBase64String(passwordInConfig);
                var entropyInByte = Convert.FromBase64String(entropyInConfig);

                var decryptedPassword = ProtectedData.Unprotect(passwordInByte, entropyInByte, DataProtectionScope.CurrentUser);
                var password = Encoding.UTF8.GetString(decryptedPassword);

                return (username, password);
            }

            return (null, null);
        }
    }
}