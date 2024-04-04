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

        private string _connectionString = "Server=.\\SQLEXPRESS;Database=MYSHOP;Trusted_Connection=yes;TrustServerCertificate=True;";

        private SqlConnection _connection;
        public RevenueDao()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        /// <summary>
        /// Lấy dữ liệu doanh thu từ Database
        /// </summary>
        /// <returns>Dữ liệu doanh thu theo ngày</returns>
        public List<RevenueModel> GetRevenuesByDay()
        {
            var sqlOrder = "SELECT ORDER_DATE, SUM(PRICE) AS REVENUE, COUNT(*) AS QUANTITY FROM ORDER_LIST GROUP BY ORDER_DATE";
            var commandOrder = new SqlCommand(sqlOrder, _connection);
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

            return Revenues;
        }

        public List<RevenueModel> GetRevenuesByMonth()
        {
            var sqlOrder = "SELECT MONTH(ORDER_DATE) AS MONTH, YEAR(ORDER_DATE) AS YEAR, SUM(PRICE) AS REVENUE, COUNT(*) AS QUANTITY FROM ORDER_LIST GROUP BY MONTH(ORDER_DATE), YEAR(ORDER_DATE) ORDER BY MONTH(ORDER_DATE)";
            var commandOrder = new SqlCommand(sqlOrder, _connection);
            var reader = commandOrder.ExecuteReader();

            while (reader.Read())
            {
                var revenue = new RevenueModel()
                {
                    Month = (int)reader["MONTH"],
                    Year = (int)reader["YEAR"],
                    Revenue = (int)reader["REVENUE"],
                    Quantity = (int)reader["QUANTITY"]
                };
                Revenues.Add(revenue);
            }
            Revenues = Revenues.OrderBy(o =>o.Month).OrderBy(o => o.Year).ToList();

            return Revenues;
        }
    }
}
