using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BookStoreManager
{
    class OrderModel : INotifyPropertyChanged
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public int price { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int credit = (int)value;
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            if (credit == 0)
            {
                return "0.000 VNĐ";
            }
            string result = credit.ToString("#,### VNĐ", cul.NumberFormat);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
