using BookStoreManager.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager
{
    public class BookShellBus
    {
        public int CurrentPage {  get; set; }
        public int TotalPages { get; set; }
        public string Search {  get; set; }
        public string Category { get; set; }
        
        public BookShellBus() {
            CurrentPage = 1; TotalPages = 0;
            Search = ""; Category = ""; 
        }
        public BindingList<CategoryModel> GetAllCategory()
        {
            var list = CategoryDao.GetCategoryListFromDB();
            list.Insert(0, new CategoryModel(0, "All"));
            return list;
        }
        public Tuple<BindingList<BookModel>, int, int, int> LoadBookList()
        {
            var (items, totalItems, totalPages) = BookDao.GetBookListFromDB(CurrentPage, 9, Search, Category);
            TotalPages = totalPages;
            CurrentPage = (TotalPages <= 0) ? 0 : CurrentPage;
            return new Tuple<BindingList<BookModel>, int, int, int>(items, totalItems, TotalPages, CurrentPage);
        }
    }
}
