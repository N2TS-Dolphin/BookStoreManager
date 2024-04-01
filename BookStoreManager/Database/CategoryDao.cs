using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookStoreManager.Database
{
    public class CategoryDao
    {
        private SqlConnection _connection;
        public CategoryDao()
        {
            string connectionString = "Server=.\\SQLEXPRESS;Database=MYSHOP;Trusted_Connection=yes;TrustServerCertificate=True;Connection Timeout=100;";
            _connection = new SqlConnection(connectionString);
            //_connection.Open();
        }
        public BindingList<CategoryModel> getCategoryList()
        {
            _connection.Open();
            BindingList<CategoryModel> result = new();
            string sql = """
                select *
                from CATEGORY
                order by CATEGORY_NAME
                """;
            var command = new SqlCommand(sql, _connection);
            //try {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int categoryID = (reader["CATEGORY_ID"] == DBNull.Value) ? -1 : (int)reader["CATEGORY_ID"];
                        string categoryName = (reader["CATEGORY_NAME"] == DBNull.Value) ? "" : (string)reader["CATEGORY_NAME"];
                        result.Add(new CategoryModel(categoryID, categoryName));
                    }
                }
            //}catch (Exception ex) { MessageBox.Show("Error!"); }
            _connection.Close();
            return result;
        }
        public BindingList<CategoryModel> getBookCategory(int bookId)
        {
            _connection.Open();
            BindingList<CategoryModel> result = new();
            string sql = """
                select *
                from CATEGORY as C 
                join BOOK_CATEGORY as BC on C.CATEGORY_ID = BC.CATEGORY_ID
                where BC.BOOK_ID = @Id
                """;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("@Id", System.Data.SqlDbType.VarChar).Value = bookId;
            //try {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int categoryID = (reader["CATEGORY_ID"] == DBNull.Value) ? -1 : (int)reader["CATEGORY_ID"];
                        string categoryName = (reader["CATEGORY_NAME"] == DBNull.Value) ? "" : (string)reader["CATEGORY_NAME"];
                        result.Add(new CategoryModel(categoryID, categoryName));
                    }
                }
            //} catch (Exception ex) { MessageBox.Show("Error!"); }
            _connection.Close();
            return result;
        }

        public int InsertNewCategory(CategoryModel category)
        {
            int result = -1;
            _connection.Open();
            string sql = "insert into CATEGORY (CATEGORY_NAME) values (@Name)";
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = category.CategoryName;
            try
            {
                command.ExecuteNonQuery();
            }catch (Exception ex){ MessageBox.Show("Insert failed."); }

            string sql2 = "select MAX(CATEGORY_ID) as id from CATEGORY";
            var command2 = new SqlCommand(sql2, _connection);

            try
            {
                using (var reader = command2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int categoryID = (reader["id"] == DBNull.Value) ? -1 : (int)reader["id"];
                        result = categoryID;
                        MessageBox.Show($"Get inserted id: {result}");
                    }
                }
            }
            catch (Exception ex){ MessageBox.Show("Cant get inserted id."); }
            _connection.Close();
            return result;
        }
        public void UpdateACategory(CategoryModel category)
        {
            _connection.Open();
            string sql = """
                update CATEGORY set CATEGORY_NAME = @Name where CATEGORY_ID = @Id
                """;
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = category.CategoryID;
            command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = category.CategoryName;

            try
            {
                command.ExecuteNonQuery();
            } catch (Exception ex){ MessageBox.Show("Update failed"); }
            _connection.Close();
        }
        public void DeleteACategory(int ID)
        {
            _connection.Open();
            string sql = """
                Delete from CATEGORY where CATEGORY_ID = @Id
                """;
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = ID;
            command.ExecuteNonQuery();
            _connection.Close();
        }
    }
}
