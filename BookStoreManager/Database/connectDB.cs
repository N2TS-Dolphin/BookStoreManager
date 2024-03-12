using BookStoreManager.Support;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.Database
{
    public class connectDB
    {
        public List<accountInfo> accounts = new List<accountInfo>();

        private string ConnectionString = "Server=DESKTOP-FNHTGP5;Database=MYSHOP;Trusted_Connection=yes;TrustServerCertificate=True;";

        /// <summary>
        /// Nhập hết tài khoản từ database
        /// </summary>
        /// <returns>Danh sách tài khoản</returns>
        public List<accountInfo> readAccount()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var sqlAccount = "SELECT * FROM Account";
                var commandAccount = new SqlCommand(sqlAccount, connection);
                var reader = commandAccount.ExecuteReader();

                while (reader.Read())
                {
                    var newAccount = new accountInfo
                    {
                        username = (string)reader["USERNAME"],
                        password = (string)reader["PASS"],
                        name = (string)reader["FULLNAME"],
                        entropy = (string)reader["ENTROPY"]
                    };
                    accounts.Add(newAccount);
                }
            }

            return accounts;
        }

        /// <summary>
        /// Thêm 1 tài khoản mới vào database
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <param name="pass">Mật khẩu người dùng</param>
        /// <param name="fullname">Tên người dùng</param>
        public void writeAccount(string username, string pass, string fullname)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                accounts = readAccount();

                var insertAccount = "INSERT INTO ACCOUNT(ACCOUNT_ID, USERNAME, PASS, ENTROPY, FULLNAME) VALUES (@account_id, @username, @pass, @entropy, @fullname)";

                var password = pass;

                var passwordInByte = Encoding.UTF8.GetBytes(password);
                var entropy = new byte[32];

                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(entropy);
                }

                var cypherText = ProtectedData.Protect(passwordInByte, entropy, DataProtectionScope.CurrentUser);

                string accountID = (string)(accounts.Count() + 1).ToString("D3");

                using (SqlCommand command = new SqlCommand(insertAccount, connection))
                {
                    command.Parameters.AddWithValue($"@account_id", accountID);
                    command.Parameters.AddWithValue($"@username", username);
                    command.Parameters.AddWithValue($"@pass", Convert.ToBase64String(cypherText));
                    command.Parameters.AddWithValue($"@entropy", Convert.ToBase64String(entropy));
                    command.Parameters.AddWithValue($"@fullname", fullname);

                    // Thực thi truy vấn INSERT
                    int rowsAffected = command.ExecuteNonQuery();

                    // Kiểm tra xem dữ liệu đã được thêm thành công hay không
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Dữ liệu đã được thêm vào bảng ACCOUNT thành công.");
                    }
                    else
                    {
                        Console.WriteLine("Không thể thêm dữ liệu vào bảng ACCOUNT.");
                    }
                }
            }
        }
    }
}
