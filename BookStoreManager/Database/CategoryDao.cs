using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.Database
{
    public class CategoryDao
    {
        private SqlConnection _connection;
        public CategoryDao()
        {
            string connectionString = "Server=.\\SQLEXPRESS;Database=MYSHOP;Trusted_Connection=yes;TrustServerCertificate=True;";
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }
        public BindingList<CategoryModel> getCategoryList()
        {
            BindingList<CategoryModel> result = new();

            string sql = """
                select *
                from CATEGORY
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
        public BindingList<CategoryModel> getBookCategory(int bookId)
        {
            BindingList<CategoryModel> result = new();
            string sql = """
                select *
                from CATEGORY as C 
                join BOOK_CATEGORY as BC on C.CATEGORY_ID = BC.CATEGORY_ID
                where BC.BOOK_ID = @Id
                """;

            var command = new SqlCommand(sql, _connection);
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
            return result;
        }


        

    }


}
