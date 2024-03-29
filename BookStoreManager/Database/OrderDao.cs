using BookStoreManager.DataType;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.Database
{
    class OrderDao
    {
        public List<OrderModel> orders = new List<OrderModel>();

        private string _connectionString = "Server=DESKTOP-FNHTGP5;Database=MYSHOP;Trusted_Connection=yes;TrustServerCertificate=True;";

        /// <summary>
        /// Đọc dữ liệu đơn hàng
        /// </summary>
        /// <returns>Danh sách đơn hàng</returns>
        public List<OrderModel> readOrders()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sqlOrder = "SELECT * FROM ORDER_LIST";
                var commandOrder = new SqlCommand(sqlOrder, connection);
                var reader = commandOrder.ExecuteReader();

                while (reader.Read())
                {
                    var newOrder = new OrderModel()
                    {
                        OrderId = (int)reader["ORDER_ID"],
                        CustomerName = (string)reader["CUSTOMER_NAME"],
                        OrderDate = (DateTime)reader["ORDER_DATE"],
                        price = (int)reader["PRICE"]
                    };
                    orders.Add(newOrder);
                }
                orders = orders.OrderBy(o => o.OrderDate).ToList();
            }
            return orders;
        }

        /// <summary>
        /// Thêm đơn hàng từ chương trình vào database
        /// </summary>
        /// <param name="CustomerName">Tên khách hàng</param>
        /// <param name="OrderDate">Ngày tạo đơn hàng</param>
        /// <param name="Price">Giá trị đơn hàng</param>
        public void writeOrder(string CustomerName, string OrderDate, int Price)
        {

        }

    }
}
