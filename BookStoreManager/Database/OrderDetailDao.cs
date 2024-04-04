using Microsoft.Data.SqlClient;
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

        private string _connectionString = "Server=.\\SQLEXPRESS;Database=MYSHOP;Trusted_Connection=yes;TrustServerCertificate=True;";

        private SqlConnection _connection;

        public OrderDetailDao()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        public void InsertOrderItems(int orderId, BindingList<OrderDetailModel> orderDetails)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    foreach (var orderDetail in orderDetails)
                    {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while inserting order items: {ex.Message}");
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

        public void DeleteOrderItems(int orderId)
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

    }
}
