﻿using BookStoreManager.DataType;
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
    public class OrderModel : INotifyPropertyChanged
    {
        public int OrderId { get; set; }
        public CustomerModel Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public int Price { get; set; }

        public string OrderAddress { get; set; }

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

    class OrderDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
