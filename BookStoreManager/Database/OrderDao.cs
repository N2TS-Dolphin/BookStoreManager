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
        public List<OrderModel> OrderInfo = new List<OrderModel>();

        private string _connectionString = "Server=DESKTOP-FNHTGP5;Database=MYSHOP;Trusted_Connection=yes;TrustServerCertificate=True;";

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
        public List<OrderModel> getOrders()
        {
            var sqlOrder = "SELECT * FROM ORDER_LIST";
            var commandOrder = new SqlCommand(sqlOrder, _connection);
            var reader = commandOrder.ExecuteReader();

            while (reader.Read())
            {
                var newOrder = new OrderModel()
                {
                    OrderId = (int)reader["ORDER_ID"],
                    CustomerName = (string)reader["CUSTOMER_NAME"],
                    OrderDate = reader.GetDateTime(reader.GetOrdinal("ORDER_DATE")),
                    Price = (int)reader["PRICE"]
                };
                OrderInfo.Add(newOrder);
            }
            OrderInfo = OrderInfo.OrderBy(o => o.OrderDate).ToList();

            return OrderInfo;
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
                            OrderDate = reader.GetDateTime(reader.GetOrdinal("ORDER_DATE")),
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
                            OrderDate = reader.GetDateTime(reader.GetOrdinal("ORDER_DATE")),
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

        public BindingList<OrderDetailModel> GetOrderDetails(int orderId)
        {
            BindingList<OrderDetailModel> orderDetails = new BindingList<OrderDetailModel>();
            BookDao _bookDao = new BookDao();

            string query = "SELECT ORDER_ID, BOOK_ID, QUANTITY FROM ORDER_ITEM WHERE ORDER_ID = @OrderId ORDER BY BOOK_ID";

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@OrderId", orderId);

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        OrderDetailModel orderDetail = new OrderDetailModel
                        {

                            OrderID = reader.GetInt32(reader.GetOrdinal("ORDER_ID")),
                            Book = _bookDao.getBookDetail(reader.GetInt32(reader.GetOrdinal("BOOK_ID"))),
                            Quantity = reader.GetInt32(reader.GetOrdinal("QUANTITY"))
                        };
                        orderDetails.Add(orderDetail);
                    }
                }
            }

            return orderDetails;
        }

        public OrderModel GetOrderById(int orderId)
        {
            string query = "SELECT ORDER_ID, CUSTOMER_NAME, ORDER_DATE, PRICE FROM ORDER_LIST WHERE ORDER_ID = @OrderId";

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@OrderId", orderId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        OrderModel order = new OrderModel
                        {
                            OrderId = reader.GetInt32(reader.GetOrdinal("ORDER_ID")),
                            CustomerName = reader.GetString(reader.GetOrdinal("CUSTOMER_NAME")),
                            OrderDate = reader.GetDateTime(reader.GetOrdinal("ORDER_DATE")),
                            Price = reader.GetInt32(reader.GetOrdinal("PRICE"))
                        };
                        return order;
                    }
                }
            }

            return null; // Return null if order with the provided orderId is not found
        }

        public void UpdateOrder(int orderId, string newCustomerName, DateTime newOrderDate, int newPrice)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "UPDATE ORDER_LIST SET CUSTOMER_NAME = @CustomerName, ORDER_DATE = @OrderDate, Price = @Price WHERE ORDER_ID = @OrderId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerName", newCustomerName);
                        command.Parameters.AddWithValue("@OrderDate", newOrderDate);
                        command.Parameters.AddWithValue("@Price", newPrice);
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the database operation
                System.Diagnostics.Debug.WriteLine($"An error occurred while updating the order: {ex.Message}");
            }
        }

    }
}