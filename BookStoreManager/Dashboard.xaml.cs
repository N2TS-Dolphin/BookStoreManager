﻿using System;
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
            btnUser.Content = database.accounts[Login.Instance.Get()].name;

            // Nếu không phải là tài khoản admin thì cho đăng ký người dùng mới
            if (Login.Instance.Get() == 0)
            {
                btnCreate.Visibility = Visibility.Visible;
                btnCreate.IsEnabled = true;
            }
            else
            {
                btnCreate.IsEnabled = false;
                btnCreate.Visibility = Visibility.Collapsed;
            }

            this.SizeChanged += Window_SizeChanged;
        }
        /// <summary>
        /// Tự động chỉnh kích thước của chữ khi thu phóng cửa sổ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Lấy kích thước thực tế của cửa sổ
            double width = slidebar.ActualWidth;
            double height = slidebar.ActualHeight;

            // Tính toán kích thước phông chữ dựa trên kích thước mới của cửa sổ
            int newSize = (int)(Math.Max(width, height) / 30);

            // Điều kiện trên và dưới cho fontsize
            if (newSize > 20)
            {
                btnUser.FontSize = 20;
                Panel_01.FontSize = 20;
                Panel_02.FontSize = 20;
                Panel_03.FontSize = 20;
                txtLogout.FontSize = 20;
                txtCreate.FontSize = 20;
            }
            else if (newSize < 15)
            {
                btnUser.FontSize = 15;
                Panel_01.FontSize = 15;
                Panel_02.FontSize = 15;
                Panel_03.FontSize = 15;
                txtLogout.FontSize = 15;
                txtCreate.FontSize = 15;
            }
            else
            {
                btnUser.FontSize = newSize;
                Panel_01.FontSize = newSize;
                Panel_02.FontSize = newSize;
                Panel_03.FontSize = newSize;
                txtLogout.FontSize = newSize;
                txtCreate.FontSize = newSize;
            }
        }
        private void Product_Click(object sender, RoutedEventArgs e)
        {
            //var bookWindow = new BookWindow();
            //bookWindow.Show();
            MainPage.Content = new ProductPage();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            CreateNewAccount screen = new CreateNewAccount();
            if (screen.ShowDialog() == true)
            {

            }
        }

        /// <summary>
        /// Logout khỏi Dashboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            //Set trạng thái chưa đăng nhập
            Login.Instance.Set(-1);

            MainWindow screen = new MainWindow();

            //Lấy màn hình hiện tại của usercontrol
            Dashboard currentScreen = (Dashboard)Dashboard.GetWindow(this);

            screen.Show();
            currentScreen.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            var orderWindow = new ManageOrder();
            orderWindow.Show();
        }
    }
}