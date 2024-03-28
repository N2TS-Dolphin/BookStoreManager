using BookStoreManager.Database;
using BookStoreManager.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookStoreManager
{
    /// <summary>
    /// Interaction logic for SlideBarDashBoard.xaml
    /// </summary>
    public partial class SlideBarDashBoard : UserControl
    {
        private Button selectedButton = null;

        AccountDao database = new AccountDao();

        public SlideBarDashBoard()
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
            double width = this.ActualWidth;
            double height = this.ActualHeight;

            // Tính toán kích thước phông chữ dựa trên kích thước mới của cửa sổ
            int newSize = (int)(Math.Max(width, height) / 30);

            // Điều kiện trên và dưới cho fontsize
            if (newSize > 24)
            {
                btnUser.FontSize = 24;
                Panel_01.FontSize = 24;
                Panel_02.FontSize = 24;
                txtLogout.FontSize = 24;
                txtCreate.FontSize = 24;
            }
            else if (newSize < 8)
            {
                btnUser.FontSize = 8;
                Panel_01.FontSize = 8;
                Panel_02.FontSize = 8;
                txtLogout.FontSize = 8;
                txtCreate.FontSize = 8;
            }    
            else
            {
                btnUser.FontSize = newSize;
                Panel_01.FontSize = newSize;
                Panel_02.FontSize = newSize;
                txtLogout.FontSize = newSize;
                txtCreate.FontSize = newSize;
            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            if(selectedButton != null && selectedButton != clickedButton)
            {
                selectedButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#006070"));
            }

            selectedButton = clickedButton;

            selectedButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0093AC"));
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            CreateNewAccount screen = new CreateNewAccount();
            if(screen.ShowDialog() == true)
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

        private void Order_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
