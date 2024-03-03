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

        readDB read = new readDB();
        public Dashboard()
        {
            InitializeComponent();
            read.accounts = read.LoadDataFromDatabase();
            btnUser.Content = read.accounts[Login.Instance.Get()].name;

            this.SizeChanged += Window_SizeChanged;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Window loginWindow = new MainWindow();
            Login.Instance.Set(-1);
            loginWindow.Show();
            this.Close();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Lấy kích thước thực tế của cửa sổ
            double width = this.ActualWidth;
            double height = this.ActualHeight;

            // Tính toán kích thước phông chữ dựa trên kích thước mới của cửa sổ
            int newSize = (int)(Math.Max(width, height) / 50); // Thay đổi 50 tùy thuộc vào yêu cầu của bạn

            // Cập nhật kích thước phông chữ của Panel_01
            btnUser.FontSize = newSize;
            Panel_01.FontSize = newSize;
            Panel_02.FontSize = newSize;
            btnLogout.FontSize = newSize;
        }

    }
}
