﻿using BookStoreManager.Database;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        public void ImportFromExcel(string filePath, BindingList<CategoryModel> categoryList)
        {
            BindingList<BookModel> books = new();
            SpreadsheetDocument document;
            try
            {
                 document = SpreadsheetDocument.Open(filePath, false);
            }catch(Exception ex) { MessageBox.Show("File excel đang được mở, vui lòng đóng file trước khi Import."); return; }
            
            var wbPart = document.WorkbookPart;
            var sheets = wbPart.Workbook.Descendants<Sheet>();

            var sheet = sheets.FirstOrDefault(s => s.Name == "Book");
            var wsPart = (WorksheetPart)(wbPart.GetPartById(sheet.Id));
            var cells = wsPart.Worksheet.Descendants<Cell>();

            int row = 2;
            Cell bookNameCell = cells.FirstOrDefault(c => c?.CellReference == $"A{row}");
            Cell authorCell = cells.FirstOrDefault(c => c?.CellReference == $"B{row}");
            Cell priceCell = cells.FirstOrDefault(c => c?.CellReference == $"C{row}");
            Cell categoryCell = cells.FirstOrDefault(c => c?.CellReference == $"D{row}");
            Cell imageCell = cells.FirstOrDefault(c => c?.CellReference == $"E{row}");

            while (bookNameCell != null && authorCell != null && priceCell != null && categoryCell != null && imageCell != null)
            {
                string bookName = GetCellValue(bookNameCell, wbPart);
                string author = GetCellValue(authorCell, wbPart);
                int price = int.Parse(GetCellValue(priceCell, wbPart));
                string category = GetCellValue(categoryCell, wbPart);
                string image = GetCellValue(imageCell, wbPart);

                BookModel newBook = new BookModel(bookName, author, price, image);
                BindingList<CategoryModel> newBookCategories = new();

                string[] categories = category.Split(",");
                foreach ( string categoryName in categories )
                {
                    CategoryModel categoryFound = new();
                    categoryFound = categoryList.FirstOrDefault(c => c.CategoryName == categoryName);
                    if(categoryFound != null )
                    {
                        newBookCategories.Add(categoryFound);
                    }
                }
                newBook.Category = newBookCategories;
                //books.Add(newBook);

                int bookID = BookDao.InsertNewBookToDB(newBook);
                newBook.BookID = bookID;
                CategoryDao.InsertNewBookCategoryToDB(newBook);

                row++;
                bookNameCell = cells.FirstOrDefault(c => c?.CellReference == $"A{row}");
                authorCell = cells.FirstOrDefault(c => c?.CellReference == $"B{row}");
                priceCell = cells.FirstOrDefault(c => c?.CellReference == $"C{row}");
                categoryCell = cells.FirstOrDefault(c => c?.CellReference == $"D{row}");
                imageCell = cells.FirstOrDefault(c => c?.CellReference == $"E{row}");
            }

            //BookDao.ImportBooksFromExcelToDB(books);
        }
        private static string GetCellValue(Cell cell, WorkbookPart workbookPart)
        {
            SharedStringTablePart stringTablePart = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString && stringTablePart != null)
            {
                return stringTablePart.SharedStringTable.ElementAt(int.Parse(cell.InnerText)).InnerText;
            }
            else
            {
                return cell.InnerText;
            }
        }
        public static BindingList<CategoryModel> CopyCategoryList(BindingList<CategoryModel> categories)
        {
            BindingList<CategoryModel> result = [.. categories];
            return result;
        }
    }
}