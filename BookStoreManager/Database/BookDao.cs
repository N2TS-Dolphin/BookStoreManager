﻿using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BookStoreManager.Database
{
    public class BookDao
    {
        private string _connectionString = DBConfig.GetConnectionString();
        private SqlConnection _connection;

        public BookDao()
        {
            _connection = new SqlConnection(_connectionString);
            while (_connection.State != ConnectionState.Open)
            {
                try
                {
                    _connection.Open();
                }
                catch (Exception ex) { }
            }
        }

        public static Tuple<string, string> GetSortValue(int sort)
        {
            var sortList = new List<string>
            {
                {"B.BOOK_ID,ASC"},
                {"B.BOOK_ID,DESC"},
                {"B.BOOK_NAME,ASC"},
                {"B.BOOK_NAME,DESC"},
                {"B.AUTHOR,ASC"},
                {"B.AUTHOR,DESC"},
                {"B.PRICE,ASC"},
                {"B.PRICE,DESC"},
            };
            var temp = sortList[sort].Split(",");
            string orderby = temp[0];
            string order = temp[1];
            return new Tuple<string, string>(orderby, order);
        }

        public static Tuple<BindingList<BookModel>, int> GetBookListFromDB(int page, int itemsPerPage, 
            string search, string category, int priceFrom, int priceTo, int sort)
        {
            var connection = DBConfig.Connection;
            BindingList<BookModel> result = new();
            int totalItems = 0; int totalPages = 0;
            var (orderby, order) = GetSortValue(sort);
            string sql = "";
            while (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();
                    sql = @"
                        SELECT B.BOOK_ID AS id, B.BOOK_NAME AS name, B.IMG AS image, B.AUTHOR AS author, B.PRICE AS price, COUNT(*) OVER() AS totalItems
                        FROM BOOK AS B 
                        JOIN BOOK_CATEGORY AS BC ON B.BOOK_ID = BC.BOOK_ID
                        JOIN CATEGORY AS C ON C.CATEGORY_ID = BC.CATEGORY_ID
                        WHERE (@Category = 'All' OR C.CATEGORY_NAME = @Category)
                        AND (@Search = '' OR B.BOOK_NAME LIKE @Search collate Latin1_General_CI_AI)
                        AND (B.PRICE BETWEEN @PriceFrom AND @PriceTo)
                        GROUP BY B.BOOK_ID, B.BOOK_NAME, B.IMG, B.AUTHOR, B.PRICE
                        ORDER BY " + orderby + " " + order + @"
                        OFFSET @Skip ROWS
                        FETCH NEXT @Take ROWS ONLY
                        ";

                    int skip = (page - 1) * itemsPerPage;
                    int take = itemsPerPage;
                    

                    var command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@Skip", System.Data.SqlDbType.Int).Value = skip;
                    command.Parameters.Add("@Take", System.Data.SqlDbType.Int).Value = take;
                    command.Parameters.Add("@Search", System.Data.SqlDbType.NVarChar).Value = "%" + search + "%";
                    command.Parameters.Add("@Category", System.Data.SqlDbType.NVarChar).Value = category;
                    command.Parameters.Add("@PriceFrom", System.Data.SqlDbType.Int).Value = priceFrom;
                    command.Parameters.Add("@PriceTo", System.Data.SqlDbType.Int).Value = priceTo;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (totalItems == 0)
                            {
                                totalItems = (int)reader["totalItems"];
                                totalItems = totalItems < 0 ? 0 : totalItems;
                                totalPages = totalItems / itemsPerPage + (totalItems % itemsPerPage == 0 ? 0 : 1);
                                totalPages = totalPages < 0 ? 0 : totalPages;
                            }
                            int bookId = (reader["id"] == DBNull.Value) ? -1 : (int)reader["id"];
                            string bookName = (reader["name"] == DBNull.Value) ? "" : (string)reader["name"];
                            string image = (reader["image"] == DBNull.Value) ? "blank_cover.jpg" : (string)reader["image"];
                            string author = (reader["author"] == DBNull.Value) ? "N/A" : (string)reader["author"];
                            int price = (reader["price"] == DBNull.Value) ? 0 : (int)reader["price"];

                            result.Add(new BookModel(bookId, bookName, price, author, image));
                        }
                    }
                }
                catch (Exception ex) { }
            }
            connection.Close();
            return new Tuple<BindingList<BookModel>, int>(result, totalPages);
        }

        public BookModel getBookDetail(int id)
        {
            BookModel result = new();
            while (_connection.State != ConnectionState.Open)
            {
                try
                {
                    _connection.Open();
                    string sql = """
                            select * from BOOK
                            where BOOK_ID = @Id
                            """;
                    var command = new SqlCommand(sql, _connection);
                    command.Parameters.Add("@Id", System.Data.SqlDbType.VarChar).Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int bookId = (reader["BOOK_ID"] == DBNull.Value) ? -1 : (int)reader["BOOK_ID"];
                            string bookName = (reader["BOOK_NAME"] == DBNull.Value) ? "" : (string)reader["BOOK_NAME"];
                            int price = (reader["PRICE"] == DBNull.Value) ? 0 : (int)reader["PRICE"];
                            string author = (reader["AUTHOR"] == DBNull.Value) ? "" : (string)reader["AUTHOR"];
                            string image = (reader["IMG"] == DBNull.Value) ? "" : (string)reader["IMG"];
                            result.BookID = bookId; result.BookName = bookName;
                            result.Author = author; result.Image = image;
                            result.Price = price;
                        }
                    }
                }
                catch (Exception ex) { result.ClearBook(); }
            }
            return result;
        }

        public static BookModel GetBookDetailFromDB(int id)
        {
            var connection = DBConfig.Connection;
            BookModel result = new();
            while (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();
                    string sql = """
                            select * from BOOK
                            where BOOK_ID = @Id
                            """;
                    var command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@Id", System.Data.SqlDbType.VarChar).Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int bookId = (reader["BOOK_ID"] == DBNull.Value) ? -1 : (int)reader["BOOK_ID"];
                            string bookName = (reader["BOOK_NAME"] == DBNull.Value) ? "" : (string)reader["BOOK_NAME"];
                            int price = (reader["PRICE"] == DBNull.Value) ? 0 : (int)reader["PRICE"];
                            string author = (reader["AUTHOR"] == DBNull.Value) ? "" : (string)reader["AUTHOR"];
                            string image = (reader["IMG"] == DBNull.Value) ? "" : (string)reader["IMG"];
                            result.BookID = bookId; result.BookName = bookName;
                            result.Author = author; result.Image = image;
                            result.Price = price;
                        }
                    }
                }
                catch (Exception ex) { result.ClearBook(); }
            }
            connection.Close();
            return result;
        }

        public static int InsertNewBookToDB(BookModel newBook)
        {
            var connection = DBConfig.Connection;
            int result = -1;
            while (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();
                    string sql = "insert into BOOK (BOOK_NAME, PRICE, AUTHOR, IMG) values (@Name, @Price, @Author, @Image)";
                    var command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = newBook.BookName;
                    command.Parameters.Add("@Price", System.Data.SqlDbType.Int).Value = newBook.Price;
                    command.Parameters.Add("@Author", System.Data.SqlDbType.NVarChar).Value = newBook.Author;
                    command.Parameters.Add("@Image", System.Data.SqlDbType.NVarChar).Value = newBook.Image;

                    command.ExecuteNonQuery();

                    string sql2 = "select MAX(BOOK_ID) as id from BOOK";
                    var command2 = new SqlCommand(sql2, connection);

                    using (var reader = command2.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int bookID = (reader["id"] == DBNull.Value) ? -1 : (int)reader["id"];
                            result = bookID;
                        }
                    }
                }
                catch (Exception ex) { }
            }
            connection.Close();
            return result;
        }

        public static void UpdateBookToDB(BookModel book)
        {
            var connection = DBConfig.Connection;
            while (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();
                    string sql = """
                update BOOK set 
                BOOK_NAME = @Name,
                PRICE = @Price,
                AUTHOR = @Author,
                IMG = @Image
                where BOOK_ID = @Id 
                """;
                    var command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@Id", System.Data.SqlDbType.NVarChar).Value = book.BookID;
                    command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = book.BookName;
                    command.Parameters.Add("@Price", System.Data.SqlDbType.Int).Value = book.Price;
                    command.Parameters.Add("@Author", System.Data.SqlDbType.NVarChar).Value = book.Author;
                    command.Parameters.Add("@Image", System.Data.SqlDbType.NVarChar).Value = book.Image;

                    command.ExecuteNonQuery();
                }
                catch (Exception ex) { }
            }
            connection.Close();
        }

        public static void DeleteBookFromDB(BookModel book)
        {
            var connection = DBConfig.Connection;
            while (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();
                    string sql = """
                delete from BOOK where BOOK_ID = @Id
                """;
                    var command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@Id", System.Data.SqlDbType.NVarChar).Value = book.BookID;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex) { }
            }
            connection.Close();
        }

        public static BindingList<BookModel> GetBooksByCategoryFromDB(int categoryId)
        {
            var connection = DBConfig.Connection;
            BindingList<BookModel> list = new BindingList<BookModel>();
            try
            {
                connection.Open();

                string sql;
                if (categoryId == 0)
                {
                    // If categoryId is 0, get all books
                    sql = "SELECT * FROM BOOK";
                }
                else
                {
                    // Otherwise, get books by category
                    sql = "SELECT B.* FROM BOOK AS B " +
                          "JOIN BOOK_CATEGORY AS BC ON B.BOOK_ID = BC.BOOK_ID " +
                          "WHERE BC.CATEGORY_ID = @CategoryID";
                }

                var command = new SqlCommand(sql, connection);

                // Add parameter if categoryId is not 0
                if (categoryId != 0)
                {
                    var sqlParameter = new SqlParameter("@CategoryID", System.Data.SqlDbType.Int);
                    sqlParameter.Value = categoryId;
                    command.Parameters.Add(sqlParameter);
                }

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var bookId = (int)reader["BOOK_ID"];
                    var bookName = (string)reader["BOOK_NAME"];
                    var author = (string)reader["AUTHOR"];
                    var price = (int)reader["PRICE"];
                    var image = reader.IsDBNull(reader.GetOrdinal("IMG")) ? "" : (string)reader["IMG"];

                    BookModel book = new BookModel()
                    {
                        BookID = bookId,
                        BookName = bookName,
                        Author = author,
                        Price = price,
                        Image = image
                    };

                    list.Add(book);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return list;
        }


        public static void ImportBooksFromExcelToDB(BindingList<BookModel> books)
        {
            var connection = DBConfig.Connection;
            while (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();
                    foreach (var book in books)
                    {
                        int result = -1;
                        string sql = "insert into BOOK (BOOK_NAME, PRICE, AUTHOR, IMG) values (@Name, @Price, @Author, @Image)";
                        var command = new SqlCommand(sql, connection);
                        command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = book.BookName;
                        command.Parameters.Add("@Price", System.Data.SqlDbType.Int).Value = book.Price;
                        command.Parameters.Add("@Author", System.Data.SqlDbType.NVarChar).Value = book.Author;
                        command.Parameters.Add("@Image", System.Data.SqlDbType.NVarChar).Value = book.Image;

                        command.ExecuteNonQuery();

                        string sql2 = "select MAX(BOOK_ID) as id from BOOK";
                        var command2 = new SqlCommand(sql2, connection);

                        using (var reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int bookID = (reader["id"] == DBNull.Value) ? -1 : (int)reader["id"];
                                result = bookID;
                            }
                        }
                        book.BookID = result;
                        CategoryDao.InsertNewBookCategoryToDB(book);
                    }
                }
                catch (Exception ex) { }
            }
            connection.Close();
        }
    }
}