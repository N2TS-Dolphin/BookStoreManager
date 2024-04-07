using BookStoreManager.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            BindingList<string> result = new BindingList<string>();
            string projectFilePath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).FullName).FullName;
            string imageFolder = Path.Combine(projectFilePath, "Image");

            // Kiểm tra xem thư mục tồn tại hay không
            if (Directory.Exists(imageFolder))
            {
                // Lấy tất cả các tên tệp trong thư mục hình ảnh
                string[] imageFiles = Directory.GetFiles(imageFolder);

                // In tên của từng tệp
                foreach (string file in imageFiles)
                {
                    result.Add(Path.GetFileName(file));
                }
            }
            else
            {
                MessageBox.Show("Thư mục hình ảnh không tồn tại.");
            }
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
