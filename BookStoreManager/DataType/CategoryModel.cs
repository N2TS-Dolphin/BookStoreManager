using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager
{
    public class CategoryModel : INotifyPropertyChanged
    {
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public CategoryModel() { }
        public CategoryModel(string categoryID, string categoryName)
        {
            CategoryID = categoryID;
            CategoryName = categoryName;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
