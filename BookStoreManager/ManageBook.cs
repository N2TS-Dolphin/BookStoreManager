using BookStoreManager.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager
{
    class ManageBook
    {
        public static BindingList<CategoryModel> GetCategories()
        {
            return CategoryDao.getCategoryList();
        }
    }
}
