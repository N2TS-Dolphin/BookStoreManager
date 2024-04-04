using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BookStoreManager
{
    public class BookModel: INotifyPropertyChanged, ICloneable
    {
        public int BookID{ get; set; }
        public string BookName { get; set; }
        public int Price { get; set; }
        public string Author { get; set; }
        public string Image { get; set; }
        public BindingList<CategoryModel>? Category { get; set; }
        public BookModel() {
            Price = 0;
            Category = new BindingList<CategoryModel>();
        }
        public BookModel(int bookID, string bookName, string image)
        {
            BookID = bookID;
            BookName = bookName;
            Image = image;
        }
        public BookModel(int bookID, string bookName, int price, string author, string image)
        {
            BookID = bookID;
            BookName = bookName;
            Price = price;
            Author = author;
            Image = image;
        }
        public BookModel ClearBook()
        {
            BookID = -1;
            BookName = "";
            Price = 0;
            Author = "";
            Image = "";
            Category.Clear();
            return this;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
    class CreditToVNDConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int credit = (int)value;
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            string result = credit.ToString("#,### VNĐ", cul.NumberFormat);
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            if (string.IsNullOrWhiteSpace(strValue))
                return 0; // or throw exception if empty string is not allowed

            string cleanedValue = Regex.Replace(strValue, @"[^\d]", "");
            if (int.TryParse(cleanedValue, out int result))
                return result;
            else
                return 0;
        }
    }
 
    class CategoryToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BindingList<CategoryModel> category = (BindingList<CategoryModel>)value;
            string result = "";
            if(category != null)
            {
                foreach (var item in category)
                {
                    result += (item == category[0]) ? $"{item.CategoryName}" : $", {item.CategoryName}";
                }
            }
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
