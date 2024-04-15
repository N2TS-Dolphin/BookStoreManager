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
        private string _connectionString = DBConfig.GetConnectionString();
        string sqlRanking = "";

        public List<ProductRankingModel> getDescList(int column, int optional)
        {
            List<ProductRankingModel> data = new List<ProductRankingModel>();
            List<string> orderby = new List<string>() { "QUANTITY DESC", "REVENUE DESC" };
            switch (optional)
            {
                case 0:
                    sqlRanking = "SELECT BOOK.BOOK_ID AS ID, BOOK.BOOK_NAME AS NAME,SUM(ORDER_ITEM.QUANTITY) AS QUANTITY , SUM(ORDER_ITEM.QUANTITY)*BOOK.PRICE AS REVENUE FROM BOOK JOIN ORDER_ITEM ON BOOK.BOOK_ID = ORDER_ITEM.BOOK_ID JOIN ORDER_LIST ON ORDER_LIST.ORDER_ID = ORDER_ITEM.ORDER_ID WHERE YEAR(ORDER_LIST.ORDER_DATE) = YEAR(GETDATE()) AND MONTH(ORDER_LIST.ORDER_DATE) = MONTH(GETDATE()) AND DAY(ORDER_LIST.ORDER_DATE) = DAY(GETDATE()) GROUP BY BOOK.BOOK_ID, BOOK.BOOK_NAME, BOOK.PRICE ORDER BY " + orderby[column];
                    break;
                case 1:
                    sqlRanking = "SELECT BOOK.BOOK_ID AS ID, BOOK.BOOK_NAME AS NAME,SUM(ORDER_ITEM.QUANTITY) AS QUANTITY , SUM(ORDER_ITEM.QUANTITY)*BOOK.PRICE AS REVENUE FROM BOOK JOIN ORDER_ITEM ON BOOK.BOOK_ID = ORDER_ITEM.BOOK_ID JOIN ORDER_LIST ON ORDER_LIST.ORDER_ID = ORDER_ITEM.ORDER_ID WHERE YEAR(ORDER_LIST.ORDER_DATE) = YEAR(GETDATE()) AND MONTH(ORDER_LIST.ORDER_DATE) = MONTH(GETDATE()) GROUP BY BOOK.BOOK_ID, BOOK.BOOK_NAME, BOOK.PRICE ORDER BY " + orderby[column];
                    break;
                case 2:
                    sqlRanking = "SELECT BOOK.BOOK_ID AS ID, BOOK.BOOK_NAME AS NAME,SUM(ORDER_ITEM.QUANTITY) AS QUANTITY , SUM(ORDER_ITEM.QUANTITY)*BOOK.PRICE AS REVENUE FROM BOOK JOIN ORDER_ITEM ON BOOK.BOOK_ID = ORDER_ITEM.BOOK_ID JOIN ORDER_LIST ON ORDER_LIST.ORDER_ID = ORDER_ITEM.ORDER_ID WHERE YEAR(ORDER_LIST.ORDER_DATE) = YEAR(GETDATE()) GROUP BY BOOK.BOOK_ID, BOOK.BOOK_NAME, BOOK.PRICE ORDER BY " + orderby[column];
                    break;
                case 3:
                    sqlRanking = "SELECT BOOK.BOOK_ID AS ID, BOOK.BOOK_NAME AS NAME,SUM(ORDER_ITEM.QUANTITY) AS QUANTITY , SUM(ORDER_ITEM.QUANTITY)*BOOK.PRICE AS REVENUE FROM BOOK JOIN ORDER_ITEM ON BOOK.BOOK_ID = ORDER_ITEM.BOOK_ID GROUP BY BOOK.BOOK_ID, BOOK.BOOK_NAME, BOOK.PRICE ORDER BY " + orderby[column];
                    break;
            }    

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

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

                    data.Add(newRanking);
                }

                reader.Close();
                connection.Close();
            }

            return data;
        }
    }
}
