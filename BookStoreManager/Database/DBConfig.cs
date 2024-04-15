using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.Database
{
    public class DBConfig
    {
        public static string ConnectionString = "Server=.\\SQLEXPRESS;Database=MYSHOP;Trusted_Connection=yes;TrustServerCertificate=True;Connection Timeout=100;";
        public static SqlConnection Connection = InitializeConnection();
        public static SqlConnection InitializeConnection()
        {
            string connectionString = GetConnectionString();
            var connection = new SqlConnection(connectionString);
            return connection;
        }
        public static string GetConnectionString()
        {
            return ConnectionString;
        }
    }
}
