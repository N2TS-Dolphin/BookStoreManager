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
            Search = ""; Category = "All"; 
        }
        public BindingList<CategoryModel> GetAllCategory()
        {
            var list = CategoryDao.GetCategoryListFromDB();
            list.Insert(0, new CategoryModel(0, "All"));
            list.OrderBy(x => x.CategoryName).ToList();
            return list;
        }
        public Tuple<BindingList<BookModel>, int, int, int> GetBookList()
        {
            var (items, totalItems, totalPages) = BookDao.GetBookListFromDB(CurrentPage, 9, Search, Category);
            TotalPages = totalPages;
            CurrentPage = (TotalPages <= 0) ? 0 : CurrentPage;
            items.OrderBy(x => x.BookName).ToList();
            return new Tuple<BindingList<BookModel>, int, int, int>(items, totalItems, TotalPages, CurrentPage);
        }
        public BindingList<CategoryModel> GetBookCategory(BookModel book)
        {
            var result = CategoryDao.GetBookCategoryFromDB(book.BookID);
            result.OrderBy(x => x.CategoryName).ToList();
            return result;
        }
        public void RefreshPage(BindingList<CategoryModel> categories)
        {
            CurrentPage = 1;
            Search = ""; Category = categories[0].CategoryName;
        }
        public void MoveToPreviousPage()
        {
            CurrentPage = (CurrentPage-- <= 1) ? 1 : CurrentPage;
        }
        public void MoveToNextPage()
        {
            CurrentPage = (CurrentPage++ >= TotalPages) ? TotalPages : CurrentPage;
        }
        public BindingList<BookModel> DeleteBook(BookModel book, BindingList<BookModel> books)
        {
            BindingList<BookModel> result = books;
            CategoryDao.DeleteAllBookCategoryFromDB(book);
            BookDao.DeleteBookFromDB(book);
            result.Remove(book);
            result.OrderBy(x => x.BookName).ToList();
            return result;
        }
        public BindingList<CategoryModel> AddCategory(CategoryModel category, BindingList<CategoryModel> categories)
        {
            BindingList <CategoryModel> result = categories;
            CategoryModel newCategory = (CategoryModel)category.Clone();
            int insertedID = CategoryDao.InsertNewCategoryToDB((CategoryModel)newCategory.Clone());
            newCategory.CategoryID = insertedID;
            result.Add(newCategory);

            result.OrderBy(x => x.CategoryName).ToList();
            return result;
        }
        public BindingList<CategoryModel> UpdateCategory(int index, CategoryModel category, BindingList<CategoryModel> categories)
        {
            BindingList<CategoryModel> result = categories;
            CategoryModel selectedCategory = (CategoryModel)category.Clone();

            CategoryDao.UpdateACategoryToDB((CategoryModel)selectedCategory.Clone());
            result[index].CategoryName = selectedCategory.CategoryName;

            result.OrderBy(x => x.CategoryName).ToList();
            return result;
        }
        public BindingList<CategoryModel> DeleteCategory(int index, BindingList<CategoryModel> categories)
        {
            BindingList<CategoryModel> result = categories;

            CategoryDao.DeleteACategoryFromDB(result[index].CategoryID);
            result.RemoveAt(index);

            result.OrderBy(x => x.CategoryName).ToList();
            return result;
        }
        public void ChangeSelectionCategory(int index, BindingList<CategoryModel> categories)
        {
            index = (index >= 0 && index < categories.Count) ? index : 0;
            Category = categories[index].CategoryName;
            CurrentPage = 1;
        }
        public void SearchBook(string search)
        {
            Search = search;
            CurrentPage = 1;
        }
    }
}
