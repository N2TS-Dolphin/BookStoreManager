using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager
{
    public class BookModel: INotifyPropertyChanged
    {
        public string BookID{ get; set; }
        public string BookName { get; set; }
        public int Price { get; set; }
        public string Author { get; set; }
        public string Image { get; set; }
        public CategoryModel[] Category { get; set; }
        public BookModel() { }
        public BookModel(string bookID, string bookName, string image)
        {
            BookID = bookID;
            BookName = bookName;
            Image = image;
        }
        public BookModel(string bookID, string bookName, int price, string author, string image, CategoryModel[] category)
        {
            BookID = bookID;
            BookName = bookName;
            Price = price;
            Author = author;
            Image = image;
            Category = category;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
