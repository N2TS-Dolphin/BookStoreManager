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

        private string _connectionString = DBConfig.GetConnectionString();

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


        public BindingList<OrderModel> GetAllOrdersFromDB()
        {
            BindingList<OrderModel> orders = new BindingList<OrderModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT ORDER_ID, CUSTOMER_ID, ORDER_DATE, PRICE FROM ORDER_LIST";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderModel order = new OrderModel
                            {
                                OrderId = reader.GetInt32(reader.GetOrdinal("ORDER_ID")),
                                Customer = GetCustomerDetailFromDB(reader.GetInt32(reader.GetOrdinal("CUSTOMER_ID"))),
                                OrderDate = reader.GetDateTime(reader.GetOrdinal("ORDER_DATE")),
                                Price = reader.GetInt32(reader.GetOrdinal("PRICE"))
                            };
                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }

        public void AddOrderToDB(CustomerModel customer, DateTime orderDate)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO ORDER_LIST (CUSTOMER_ID, ORDER_DATE) VALUES (@CustomerId, @OrderDate)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customer.CustomerID);
                    command.Parameters.AddWithValue("@OrderDate", orderDate);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Order added successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to add order: " + ex.Message);
                    }
                }
            }
        }

        public Tuple<BindingList<OrderModel>, int, int> GetAllPagingFromDB(int page, int rowsPerPage, DateTime? fromDate, DateTime? toDate)
        {
            BindingList<OrderModel> orders = new BindingList<OrderModel>();
            int totalItems = -1;
            int totalPages = 0;
            string where = "";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    if (fromDate != null && toDate != null)
                    {
                        where += " WHERE ORDER_DATE BETWEEN @FromDate AND @ToDate";
                    }

                    // Calculate skip and take values for pagination
                    int skip = (page - 1) * rowsPerPage;
                    int take = rowsPerPage;

                    // SQL query for retrieving orders with pagination and optional filtering
                    string query = $@"
                    SELECT ORDER_ID, CUSTOMER_ID, ORDER_DATE, PRICE, 
                           COUNT(*) OVER() AS TotalItems
                    FROM ORDER_LIST
                    {where}
                    ORDER BY ORDER_ID
                    OFFSET @Skip ROWS
                    FETCH NEXT @Take ROWS ONLY";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters for pagination
                        command.Parameters.Add("@Skip", SqlDbType.Int).Value = skip;
                        command.Parameters.Add("@Take", SqlDbType.Int).Value = take;

                        // Add parameter for the keyword if provided
                        if (fromDate != null && toDate != null)
                        {
                            command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate.Value;
                            command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate.Value;
                        }

                        connection.Open();

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
                                    Customer = GetCustomerDetailFromDB(reader.GetInt32(reader.GetOrdinal("CUSTOMER_ID"))),
                                    OrderDate = reader.GetDateTime(reader.GetOrdinal("ORDER_DATE")),
                                    Price = reader.GetInt32(reader.GetOrdinal("PRICE"))
                                };
                                orders.Add(order);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while retrieving orders: {ex.Message}");
                }
            }

            return new Tuple<BindingList<OrderModel>, int, int>(orders, totalItems, totalPages);
        }

        public CustomerModel GetCustomerDetailFromDB(int customerId)
        {
            CustomerModel customer = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT CUSTOMER_ID, CUSTOMER_NAME, CUSTOMER_EMAIL, CUSTOMER_PHONE FROM CUSTOMER WHERE CUSTOMER_ID = @CustomerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int id = reader.GetInt32(reader.GetOrdinal("CUSTOMER_ID"));
                                string name = reader.GetString(reader.GetOrdinal("CUSTOMER_NAME"));
                                string email = reader.IsDBNull(reader.GetOrdinal("CUSTOMER_EMAIL")) ? null : reader.GetString(reader.GetOrdinal("CUSTOMER_EMAIL"));
                                string phone = reader.IsDBNull(reader.GetOrdinal("CUSTOMER_PHONE")) ? null : reader.GetString(reader.GetOrdinal("CUSTOMER_PHONE"));

                                customer = new CustomerModel(id, name, email, phone);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred while retrieving customer details: {ex.Message}");
                    }
                }
            }

            return customer;
        }

        public void DeleteOrderFromDB(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string deleteOrderItemsSql = "DELETE FROM order_item WHERE ORDER_ID = @OrderId";
                string deleteOrderSql = "DELETE FROM ORDER_LIST WHERE ORDER_ID = @OrderId";

                try
                {
                    connection.Open();

                    // Delete items associated with the order from order_item table
                    using (SqlCommand deleteOrderItemsCommand = new SqlCommand(deleteOrderItemsSql, connection))
                    {
                        deleteOrderItemsCommand.Parameters.AddWithValue("@OrderId", orderId);
                        deleteOrderItemsCommand.ExecuteNonQuery();
                    }

                    // Delete the order itself from ORDER_LIST table
                    using (SqlCommand deleteOrderCommand = new SqlCommand(deleteOrderSql, connection))
                    {
                        deleteOrderCommand.Parameters.AddWithValue("@OrderId", orderId);
                        deleteOrderCommand.ExecuteNonQuery();
                    }

                    Console.WriteLine("Order and associated items deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete order and associated items: {ex.Message}");
                }
            }
        }


        public void AddOrderToDB(int customerId, DateTime orderDate)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string insertOrderSql = "INSERT INTO ORDER_LIST (CUSTOMER_ID, ORDER_DATE) VALUES (@CustomerId, @OrderDate)";

                using (SqlCommand command = new SqlCommand(insertOrderSql, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.Parameters.AddWithValue("@OrderDate", orderDate);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Order added successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to add order: {ex.Message}");
                    }
                }
            }
        }

        public OrderModel GetOrderByIdFromDB(int orderId)
        {
            OrderModel order = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT ORDER_ID, CUSTOMER_ID, ORDER_DATE, PRICE FROM ORDER_LIST WHERE ORDER_ID = @OrderId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderId", orderId);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                order = new OrderModel
                                {
                                    OrderId = reader.GetInt32(reader.GetOrdinal("ORDER_ID")),
                                    Customer = GetCustomerDetailFromDB(reader.GetInt32(reader.GetOrdinal("CUSTOMER_ID"))),
                                    OrderDate = reader.GetDateTime(reader.GetOrdinal("ORDER_DATE")),
                                    Price = reader.GetInt32(reader.GetOrdinal("PRICE"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the order by ID: {ex.Message}");
            }

            return order;
        }

        public void UpdateOrderToDB(int orderId, int newCustomerId, DateTime newOrderDate, int newPrice)
{
    try
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "UPDATE ORDER_LIST SET CUSTOMER_ID = @CustomerId, ORDER_DATE = @OrderDate, PRICE = @Price WHERE ORDER_ID = @OrderId";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CustomerId", newCustomerId);
                command.Parameters.AddWithValue("@OrderDate", newOrderDate);
                command.Parameters.AddWithValue("@Price", newPrice);
                command.Parameters.AddWithValue("@OrderId", orderId);
                command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while updating the order: {ex.Message}");
        // Handle or log the exception as needed
    }
}




    }
}