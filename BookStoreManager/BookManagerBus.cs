using BookStoreManager.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager
{
    class BookManagerBus
    {
        public static BindingList<CategoryModel> GetCategories()
        {
            return CategoryDao.GetCategoryListFromDB();
        }
        public static BindingList<string> GetImageName()
        {
            BindingList<string> result = new BindingList<string>
            {
                {"blank_cover.jpg" },
                {"OnePieceCover.jpg" }
            };
            return result;
        }
        public static int AddNewBook(BookModel newBook)
        {
            int bookID = BookDao.InsertNewBookToDB(newBook);
            newBook.BookID = bookID;
            CategoryDao.InsertNewBookCategoryToDB(newBook);
            return newBook.BookID;
        }
        public static BindingList<CategoryModel> GetUnuseCategory(BookModel book)
        {
            BindingList <CategoryModel> result = CategoryDao.GetUnuseCategoriesFromDB(book);
            result.OrderBy(x => x.CategoryName).ToList();
            return result;
        }
        public static void UpdateBook(BookModel book, BindingList<CategoryModel> deleteCategories, BindingList<CategoryModel> insertCategories)
        {
            CategoryDao.DeleteOldBookCategoryFromDB(book, deleteCategories);
            CategoryDao.InsertNewBookCategoryToDB(book, insertCategories);
            BookDao.UpdateBookToDB(book);
        }
        public static BindingList<CategoryModel> CopyCategoryList(BindingList<CategoryModel> categories)
        {
            BindingList<CategoryModel> result = [.. categories];
            return result;
        }
        public static BookModel CopyBook(BookModel SelectedBook)
        {
            BookModel result = new BookModel();
            result = SelectedBook;
            return result;
        }
    }
}
