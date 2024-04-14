using Microsoft.Data.SqlClient;
using BookStoreManager.Process;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.Database
{
    class OrderDetailDao
    {

        private string _connectionString = DBConfig.GetConnectionString();

        private SqlConnection _connection;

        public OrderDetailDao()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        public void InsertOrderItemToDB(int orderId, OrderDetailModel orderDetail)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    
                    // Insert each order detail into the database
                    string insertQuery = "INSERT INTO ORDER_ITEM (ORDER_ID, BOOK_ID, QUANTITY) VALUES (@OrderId, @BookId, @Quantity)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        command.Parameters.AddWithValue("@BookId", orderDetail.Book.BookID);
                        command.Parameters.AddWithValue("@Quantity", orderDetail.Quantity);
                        command.ExecuteNonQuery();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while inserting order items: {ex.Message}");
            }
        }

        public BindingList<OrderDetailModel> GetOrderDetailsFromDB(int orderId)
        {
            BindingList<OrderDetailModel> orderDetails = new BindingList<OrderDetailModel>();
            BookShellBus BookShell= new BookShellBus();

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
                            Book = BookShell.GetBookDetail(reader.GetInt32(reader.GetOrdinal("BOOK_ID"))),
                            Quantity = reader.GetInt32(reader.GetOrdinal("QUANTITY"))
                        };
                        orderDetails.Add(orderDetail);
                    }
                }
            }

            return orderDetails;
        }

        public void DeleteOrderItemsFromDB(int orderId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Delete order items associated with the given orderId
                    string deleteQuery = "DELETE FROM ORDER_ITEM WHERE ORDER_ID = @OrderId";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting order items: {ex.Message}");
            }
        }

        public void DeleteOrderDetailsFromDB(int orderID)
        {
            var sql = "delete from ORDER_ITEM where ORDER_ID = @OrderID";
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

    }
}
