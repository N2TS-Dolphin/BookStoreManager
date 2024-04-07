using BookStoreManager.DataType;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.Database
{
    public class ProductRankingDao
    {
        public List<ProductRankingModel> Ranking = new List<ProductRankingModel>();
        private string _connectionString = "Server=.\\SQLEXPRESS;Database=MYSHOP;Trusted_Connection=yes;TrustServerCertificate=True;"; //Change Server

        public List<ProductRankingModel> rankingList()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sqlRanking = "SELECT BOOK.BOOK_ID AS ID, BOOK.BOOK_NAME AS NAME,SUM(ORDER_ITEM.QUANTITY) AS QUANTITY , SUM(ORDER_ITEM.QUANTITY)*BOOK.PRICE AS REVENUE FROM BOOK JOIN ORDER_ITEM ON BOOK.BOOK_ID = ORDER_ITEM.BOOK_ID GROUP BY BOOK.BOOK_ID, BOOK.BOOK_NAME, BOOK.PRICE";
                var commandOrder = new SqlCommand(sqlRanking, connection);
                var reader = commandOrder.ExecuteReader();

                while (reader.Read())
                {
                    var newRanking = new ProductRankingModel()
                    {
                        Id = (int)reader["ID"],
                        Name = (string)reader["NAME"],
                        Quantity = (int)reader["QUANTITY"],
                        Revenue = (int)reader["REVENUE"]
                    };

                    Ranking.Add(newRanking);
                }

                reader.Close();
                connection.Close();
            }    
            Ranking = Ranking.OrderBy(x => x.Id).ToList();

            return Ranking;
        }
    }
}
