using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager
{
    class OrderDao
    {
        private SqlConnection _connection;
        public OrderDao()
        {
            string connectionString = "Server=.\\SQLEXPRESS;Database=MYSHOP;Trusted_Connection=yes;TrustServerCertificate=True;";
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }
    }
}
