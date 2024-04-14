using BookStoreManager.DataType;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BookStoreManager.Database
{
    public class CustomerDao
    {
        private string _connectionString = DBConfig.GetConnectionString();
        private SqlConnection _connection;

        public CustomerDao()
        {
            _connection = new SqlConnection(_connectionString);
        }

        public Tuple<BindingList<CustomerModel>, int> GetCustomerListFromDB(int page, int itemsPerPage, string search)
        {
            BindingList<CustomerModel> result = new();
            int totalItems = 0; int totalPages = 0;
            while (_connection.State != ConnectionState.Open)
            {
                try
                {
                    _connection.Open();
                    string sql = """
                        select CUSTOMER_ID as id, CUSTOMER_NAME as name, CUSTOMER_EMAIL as email, CUSTOMER_PHONE as phone, COUNT(*) OVER() AS totalItems
                        from CUSTOMER
                        where (@Search = '' OR CUSTOMER_NAME LIKE @Search collate Latin1_General_CI_AI)
                        order by CUSTOMER_ID
                        OFFSET @Skip ROWS
                        FETCH NEXT @Take ROWS ONLY
                        """;

                    int skip = (page - 1) * itemsPerPage;
                    int take = itemsPerPage;
                    var command = new SqlCommand(sql, _connection);
                    command.Parameters.Add("@Skip", System.Data.SqlDbType.Int).Value = skip;
                    command.Parameters.Add("@Take", System.Data.SqlDbType.Int).Value = take;
                    command.Parameters.Add("@Search", System.Data.SqlDbType.NVarChar).Value = "%" + search + "%";

                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            if (totalItems == 0)
                            {
                                totalItems = (int)reader["totalItems"];
                                totalItems = totalItems < 0 ? 0 : totalItems;
                                totalPages = totalItems / itemsPerPage + (totalItems % itemsPerPage == 0 ? 0 : 1);
                                totalPages = totalPages < 0 ? 0 : totalPages;
                            }
                            int id = (reader["id"] == DBNull.Value) ? -1 : (int)reader["id"];
                            string name = (reader["name"] == DBNull.Value) ? "N/A" : (string)reader["name"];
                            string email = (reader["email"] == DBNull.Value) ? "N/A" : (string)reader["email"];
                            string phone = (reader["phone"] == DBNull.Value) ? "N/A" : (string)reader["phone"];

                            result.Add(new CustomerModel(id, name, email, phone));
                        }
                    }
                }
                catch (Exception ex) { }
            }
            _connection.Close();
            return new Tuple<BindingList<CustomerModel>, int>(result, totalPages);
        }


        public void InsertCustomerIntoDB(CustomerModel customer)
        {
            while (_connection.State != ConnectionState.Open)
            {
                try
                {
                    _connection.Open();
                    string sql = """
                        insert into CUSTOMER (CUSTOMER_NAME, CUSTOMER_EMAIL, CUSTOMER_PHONE)
                        values (@Name, @Email, @Phone)
                        """;
                    var command = new SqlCommand(sql, _connection);
                    command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = customer.CustomerName;
                    command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar).Value = customer.CustomerEmail;
                    command.Parameters.Add("@Phone", System.Data.SqlDbType.VarChar).Value = customer.CustomerPhone;

                    command.ExecuteNonQuery();
                }
                catch (Exception ex) { }
            }
            _connection.Close();
        }

        public void UpdateCustomerToDB(CustomerModel customer)
        {
            while (_connection.State != ConnectionState.Open)
            {
                try
                {
                    _connection.Open();
                    string sql = """
                        update CUSTOMER set CUSTOMER_NAME = @Name, CUSTOMER_EMAIL = @Email, CUSTOMER_PHONE = @Phone
                        where CUSTOMER_ID = @Id
                        """;
                    var command = new SqlCommand(sql, _connection);
                    command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = customer.CustomerID;
                    command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = customer.CustomerName;
                    command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar).Value = customer.CustomerEmail;
                    command.Parameters.Add("@Phone", System.Data.SqlDbType.VarChar).Value = customer.CustomerPhone;

                    command.ExecuteNonQuery();
                }
                catch (Exception ex) { }
            }
            _connection.Close();
        }

        public void DeleteCustomerFromDB(CustomerModel customer)
        {
            while (_connection.State != ConnectionState.Open)
            {
                //try
                //{
                    _connection.Open();
                    string sql = """
                        delete from CUSTOMER
                        where CUSTOMER_ID = @Id
                        """;
                    var command = new SqlCommand(sql, _connection);
                    command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = customer.CustomerID;

                    command.ExecuteNonQuery();
                //}
                //catch (Exception ex) { }
            }
            _connection.Close();
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
                        // Handle any exceptions that may occur during the database operation
                        Console.WriteLine($"An error occurred while retrieving customer details: {ex.Message}");
                    }
                }
            }

            return customer;
        }

        public BindingList<CustomerModel> GetAllCustomersFromDB(string search)
        {
            BindingList<CustomerModel> result = new BindingList<CustomerModel>();

            try
            {
                _connection.Open();
                string sql = @"
                    SELECT CUSTOMER_ID AS id, CUSTOMER_NAME AS name, CUSTOMER_EMAIL AS email, CUSTOMER_PHONE AS phone
                    FROM CUSTOMER
                    WHERE @Search = '' OR CUSTOMER_NAME LIKE @Search COLLATE Latin1_General_CI_AI
                    ORDER BY CUSTOMER_ID";

                var command = new SqlCommand(sql, _connection);
                command.Parameters.Add("@Search", SqlDbType.NVarChar).Value = "%" + search + "%";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.IsDBNull(reader.GetOrdinal("id")) ? -1 : reader.GetInt32(reader.GetOrdinal("id"));
                        string name = reader.IsDBNull(reader.GetOrdinal("name")) ? "N/A" : reader.GetString(reader.GetOrdinal("name"));
                        string email = reader.IsDBNull(reader.GetOrdinal("email")) ? "N/A" : reader.GetString(reader.GetOrdinal("email"));
                        string phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? "N/A" : reader.GetString(reader.GetOrdinal("phone"));

                        result.Add(new CustomerModel(id, name, email, phone));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving customers: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }

            return result;
        }

    }
}
