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
    }
}
