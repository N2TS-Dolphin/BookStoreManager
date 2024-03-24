using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager
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
                    string categoryID = (reader["CATEGORY_ID"] == DBNull.Value) ? "" : (string)reader["CATEGORY_ID"];
                    string categoryName = (reader["CATEGORY_NAME"] == DBNull.Value) ? "" : (string)reader["CATEGORY_NAME"];
                    result.Add(new CategoryModel(categoryID, categoryName));
                }
            }
            return result;
        }
    }
}
