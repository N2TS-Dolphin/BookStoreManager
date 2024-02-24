using BookStoreManager.Support;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookStoreManager.Database
{
    public class readDB
    {
        public List<accountInfo>? accounts { get; set; }
        public List<accountInfo> LoadDataFromDatabase()
        {
            List<accountInfo> accounts = new List<accountInfo>();
            using (MySqlConnection connection = DBUtils.GetDBConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT * FROM datatest.account", connection);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string user = reader.GetString("username");
                            string pass = reader.GetString("password");
                            string name = reader.GetString("name");

                            accountInfo temp = new accountInfo()
                            {
                                username = user,
                                password = pass,
                                name = name
                            };

                            accounts.Add(temp);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            return accounts;
        }
    }
}
