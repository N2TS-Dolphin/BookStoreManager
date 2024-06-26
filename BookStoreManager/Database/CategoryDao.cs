﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BookStoreManager.Database
{
    public class CategoryDao
    {
        private string _connectionString = DBConfig.GetConnectionString();
        private SqlConnection _connection;
        public CategoryDao()
        {
            _connection = new SqlConnection(_connectionString);
            while (_connection.State != ConnectionState.Open) { try { _connection.Open(); } catch (Exception ex) { } }
        }
        public BindingList<CategoryModel> getCategoryList()
        {
            BindingList<CategoryModel> result = new();
            string sql = """
                select *
                from CATEGORY
                order by CATEGORY_NAME
                """;
            var command = new SqlCommand(sql, _connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int categoryID = (reader["CATEGORY_ID"] == DBNull.Value) ? -1 : (int)reader["CATEGORY_ID"];
                    string categoryName = (reader["CATEGORY_NAME"] == DBNull.Value) ? "" : (string)reader["CATEGORY_NAME"];
                    result.Add(new CategoryModel(categoryID, categoryName));
                }
            }
            return result;
        }
        public static BindingList<CategoryModel> GetCategoryListFromDB()
        {
            var connection = DBConfig.Connection;
            BindingList<CategoryModel> result = new();
            while (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();

                    string sql = """
                                select *
                                from CATEGORY
                                order by CATEGORY_NAME
                                """;
                    var command = new SqlCommand(sql, connection);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int categoryID = (reader["CATEGORY_ID"] == DBNull.Value) ? -1 : (int)reader["CATEGORY_ID"];
                            string categoryName = (reader["CATEGORY_NAME"] == DBNull.Value) ? "" : (string)reader["CATEGORY_NAME"];
                            result.Add(new CategoryModel(categoryID, categoryName));
                        }
                    }
                }
                catch (Exception ex) { result.Clear(); }
            }
            connection.Close();
            return result;
        }
        public static BindingList<CategoryModel> GetBookCategoryFromDB(int bookId)
        {
            var connection = DBConfig.Connection;
            BindingList<CategoryModel> result = new();
            while (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();

                    string sql = """
                            select *
                            from CATEGORY as C 
                            join BOOK_CATEGORY as BC on C.CATEGORY_ID = BC.CATEGORY_ID
                            where BC.BOOK_ID = @Id
                            """;

                    var command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@Id", System.Data.SqlDbType.VarChar).Value = bookId;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int categoryID = (reader["CATEGORY_ID"] == DBNull.Value) ? -1 : (int)reader["CATEGORY_ID"];
                            string categoryName = (reader["CATEGORY_NAME"] == DBNull.Value) ? "" : (string)reader["CATEGORY_NAME"];
                            result.Add(new CategoryModel(categoryID, categoryName));
                        }
                    }
                }
                catch (Exception ex) { result.Clear(); }
            }
            connection.Close();
            return result;
        }
        public static int InsertNewCategoryToDB(CategoryModel category)
        {
            var connection = DBConfig.Connection;
            int result = -1;
            while (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();

                    string sql = "insert into CATEGORY (CATEGORY_NAME) values (@Name)";
                    var command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = category.CategoryName;

                    command.ExecuteNonQuery();

                    string sql2 = "select MAX(CATEGORY_ID) as id from CATEGORY";
                    var command2 = new SqlCommand(sql2, connection);

                    using (var reader = command2.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int categoryID = (reader["id"] == DBNull.Value) ? -1 : (int)reader["id"];
                            result = categoryID;
                        }
                    }
                }
                catch (Exception ex) { }
            }
            connection.Close();
            return result;
        }
        public static void UpdateACategoryToDB(CategoryModel category)
        {
            var connection = DBConfig.Connection;
            while (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();

                    string sql = """
                            update CATEGORY set CATEGORY_NAME = @Name where CATEGORY_ID = @Id
                            """;
                    var command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = category.CategoryID;
                    command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = category.CategoryName;

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex) { MessageBox.Show("Update failed"); }
                }
                catch (Exception ex) { }
            }
            connection.Close();
        }
        public static void DeleteACategoryFromDB(int ID)
        {
            var connection = DBConfig.Connection;
            while (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();


                    string sql1 = """
                            Delete from BOOK_CATEGORY where CATEGORY_ID = @Id
                            """;
                    var command1 = new SqlCommand(sql1, connection);
                    command1.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = ID;
                    command1.ExecuteNonQuery();

                    string sql2 = """
                            Delete from CATEGORY where CATEGORY_ID = @Id
                            """;
                    var command2 = new SqlCommand(sql2, connection);
                    command2.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = ID;
                    command2.ExecuteNonQuery();
                }
                catch (Exception ex) { }
            }
            connection.Close();
        }
        public static void InsertNewBookCategoryToDB(BookModel book)
        {
            var connection = DBConfig.Connection;
            while (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();

                    foreach (var category in book.Category)
                    {
                        string sql = """
                                insert into BOOK_CATEGORY (BOOK_ID, CATEGORY_ID) values (@BookID, @CategoryID)
                                """;
                        var command = new SqlCommand(sql, connection);
                        command.Parameters.Add("@BookID", System.Data.SqlDbType.Int).Value = book.BookID;
                        command.Parameters.Add("@CategoryID", System.Data.SqlDbType.Int).Value = category.CategoryID;
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) { }
            }
            connection.Close();
        }
        public static BindingList<CategoryModel> GetUnuseCategoriesFromDB(BookModel book)
        {
            var connection = DBConfig.Connection;
            BindingList<CategoryModel> result = new();
            while (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();
                    string sql = """
                            select * 
                            from Category 
                            where CATEGORY_ID not in (
                            select CATEGORY_ID 
                            from BOOK_CATEGORY 
                            where BOOK_ID = @Id
                            )
                            """;
                    var command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@Id", System.Data.SqlDbType.VarChar).Value = book.BookID;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int categoryID = (reader["CATEGORY_ID"] == DBNull.Value) ? -1 : (int)reader["CATEGORY_ID"];
                            string categoryName = (reader["CATEGORY_NAME"] == DBNull.Value) ? "" : (string)reader["CATEGORY_NAME"];
                            result.Add(new CategoryModel(categoryID, categoryName));
                        }
                    }
                }
                catch (Exception ex) { result.Clear(); }
            }
            connection.Close();
            return result;
        }
        public static void InsertNewBookCategoryToDB(BookModel book, BindingList<CategoryModel> insertCategories)
        {
            var connection = DBConfig.Connection;
            while (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();

                    foreach (var category in insertCategories)
                    {
                        string sql = """
                                insert into BOOK_CATEGORY (BOOK_ID, CATEGORY_ID) values (@BookID, @CategoryID)
                                """;
                        var command = new SqlCommand(sql, connection);
                        command.Parameters.Add("@BookID", System.Data.SqlDbType.Int).Value = book.BookID;
                        command.Parameters.Add("@CategoryID", System.Data.SqlDbType.Int).Value = category.CategoryID;
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) { }
            }
            connection.Close();
        }
        public static void DeleteOldBookCategoryFromDB(BookModel book, BindingList<CategoryModel> deleteCategories)
        {
            var connection = DBConfig.Connection;
            while (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();

                    foreach (var category in deleteCategories)
                    {
                        string sql = """
                                delete from BOOK_CATEGORY where CATEGORY_ID = @CategoryID and BOOK_ID = @BookID
                                """;
                        var command = new SqlCommand(sql, connection);
                        command.Parameters.Add("@BookID", System.Data.SqlDbType.Int).Value = book.BookID;
                        command.Parameters.Add("@CategoryID", System.Data.SqlDbType.Int).Value = category.CategoryID;
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) { }
            }
            connection.Close();
        }
        public static void DeleteAllBookCategoryFromDB(BookModel book)
        {
            var connection = DBConfig.Connection;
            while (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();

                    string sql = """
                            delete from BOOK_CATEGORY where BOOK_ID = @BookID
                            """;
                    var command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@BookID", System.Data.SqlDbType.Int).Value = book.BookID;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex) { }
            }
            connection.Close();
        }
    }
}
