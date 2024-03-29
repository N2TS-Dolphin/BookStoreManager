using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BookStoreManager.DataType;
using System.Data.Common;

namespace BookStoreManager.Database
{
    class OrderDao
    {
        public List<OrderModel> orders = new List<OrderModel>();

        private string _connectionString = "Server=.\\SQLEXPRESS;Database=MYSHOP;Trusted_Connection=yes;TrustServerCertificate=True;";

        private SqlConnection _connection;

        public OrderDao()
        { 
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }


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
                        OrderDate = DateOnly.Parse(DateTime.Parse(reader["ORDER_DATE"].ToString()).Date.ToShortDateString()),
                        Price = (int)reader["PRICE"]
                    };
                    orders.Add(newOrder);
                }
                orders = orders.OrderBy(o => o.OrderDate).ToList();
            }
            return orders;
        }

        public BindingList<OrderModel> GetAllOrders()
        {
            BindingList<OrderModel> orders = new BindingList<OrderModel>();

            string query = "SELECT ORDER_ID, CUSTOMER_NAME, ORDER_DATE, PRICE FROM ORDER_LIST";

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        OrderModel order = new OrderModel
                        {
                            OrderId = reader.GetInt32(reader.GetOrdinal("ORDER_ID")),
                            CustomerName = reader.GetString(reader.GetOrdinal("CUSTOMER_NAME")),
                            OrderDate = DateOnly.Parse(DateTime.Parse(reader["ORDER_DATE"].ToString()).Date.ToShortDateString()),
                            Price = reader.GetInt32(reader.GetOrdinal("PRICE"))
                        };
                        orders.Add(order);
                    }
                }
            }

            return orders;
        }

        public Tuple<BindingList<OrderModel>, int, int> GetAllPaging(int page, int rowsPerPage, DateTime? fromDate, DateTime? toDate)
        {
            BindingList<OrderModel> orders = new BindingList<OrderModel>();
            int totalItems = -1;
            int totalPages = 0;
            string where = "";

            if (fromDate != null && toDate != null)
            {
                where += " WHERE ORDER_DATE BETWEEN @FromDate AND @ToDate";
            }

            // Calculate skip and take values for pagination
            int skip = (page - 1) * rowsPerPage;
            int take = rowsPerPage;

            // SQL query for retrieving orders with pagination and optional filtering
            string query = $@"
                SELECT ORDER_ID, CUSTOMER_NAME, ORDER_DATE, PRICE, 
                       COUNT(*) OVER() AS TotalItems
                FROM ORDER_LIST
                {where}
                ORDER BY ORDER_ID
                OFFSET @Skip ROWS
                FETCH NEXT @Take ROWS ONLY";

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                // Add parameters for pagination
                command.Parameters.Add("@Skip", System.Data.SqlDbType.Int).Value = skip;
                command.Parameters.Add("@Take", System.Data.SqlDbType.Int).Value = take;

                // Add parameter for the keyword if provided
                if (fromDate != null && toDate != null)
                {
                    command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate.Value;
                    command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate.Value;
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // If totalItems is not set yet, retrieve it from the first row
                        if (totalItems == -1)
                        {
                            totalItems = (int)reader["TotalItems"];
                            totalPages = (totalItems / rowsPerPage) + ((totalItems % rowsPerPage == 0) ? 0 : 1);
                        }

                        OrderModel order = new OrderModel
                        {
                            OrderId = reader.GetInt32(reader.GetOrdinal("ORDER_ID")),
                            CustomerName = reader.GetString(reader.GetOrdinal("CUSTOMER_NAME")),
                            OrderDate = DateOnly.Parse(reader.GetDateTime(reader.GetOrdinal("ORDER_DATE")).ToShortDateString()),
                            Price = reader.GetInt32(reader.GetOrdinal("PRICE"))
                        };
                        orders.Add(order);
                    }
                }
            }

            return new Tuple<BindingList<OrderModel>, int, int>(orders, totalItems, totalPages);
        }

        public void DeleteOrder(int orderID)
        {
            var sql = "delete from ORDER_LIST where ORDER_ID = @OrderID";
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);

            sqlCommand.Parameters.AddWithValue("@OrderID", orderID);

            try
            {
                sqlCommand.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"Deleted {orderID} Successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Deleted {orderID} Fail: " + ex.Message);
            }
        }

        public void AddOrder(string customerName, DateTime orderDate)
        {
            string query = "INSERT INTO ORDER_LIST (CUSTOMER_NAME, ORDER_DATE) VALUES (@CustomerName, @OrderDate)";

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@CustomerName", customerName);
                command.Parameters.AddWithValue("@OrderDate", orderDate);

                try
                {
                    command.ExecuteNonQuery();
                    System.Diagnostics.Debug.WriteLine("Order added successfully.");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Failed to add order: " + ex.Message);
                }
            }
        }
    }
}
