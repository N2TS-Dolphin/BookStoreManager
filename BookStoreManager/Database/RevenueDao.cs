using BookStoreManager.DataType;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.Database
{
    class RevenueDao
    {
        public List<RevenueModel> Revenues = new List<RevenueModel>();

        private string _connectionString = "Server=DESKTOP-FNHTGP5;Database=MYSHOP;Trusted_Connection=yes;TrustServerCertificate=True;";

        /// <summary>
        /// Lấy dữ liệu doanh thu từ Database
        /// </summary>
        /// <returns>Dữ liệu doanh thu theo ngày</returns>
        public List<RevenueModel> GetRevenues()
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sqlOrder = "SELECT ORDER_DATE, SUM(PRICE) AS REVENUE, COUNT(*) AS QUANTITY FROM ORDER_LIST GROUP BY ORDER_DATE";
                var commandOrder = new SqlCommand(sqlOrder, connection);
                var reader = commandOrder.ExecuteReader();

                while (reader.Read())
                {
                    var newRevenue = new RevenueModel()
                    {
                        OrderDate = (DateTime)reader["ORDER_DATE"],
                        Revenue = (int)reader["REVENUE"],
                        Quantity = (int)reader["QUANTITY"]
                    };
                    Revenues.Add(newRevenue);
                }
                Revenues = Revenues.OrderBy(o => o.OrderDate).ToList();
            }
            return Revenues;
        }
    }
}
