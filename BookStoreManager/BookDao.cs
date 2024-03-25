using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager
{
    public class BookDao
    {
        private SqlConnection _connection;
        public BookDao()
        {
            string connectionString = "Server=.\\SQLEXPRESS;Database=MYSHOP;Trusted_Connection=yes;TrustServerCertificate=True;";
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }
        public Tuple<BindingList<BookModel>, int, int> getBookList(int page, int itemsPerPage, string search, string category)
        {
            BindingList<BookModel> result = new();
            int totalItems = 0; int totalPages = 0;
            string sql = "";
            if(search != "" && category != "")
            {
                sql = """
                    select B.BOOK_ID as id, B.BOOK_NAME as name, B.IMG as image, count(*) over() as totalItems
                    from BOOK as B 
                    join BOOK_CATEGORY as BC on B.BOOK_ID = BC.BOOK_ID
                    join CATEGORY as C on C.CATEGORY_ID = BC.CATEGORY_ID
                    where C.CATEGORY_NAME = @Category
                    and B.BOOK_NAME like @Search
                    group by B.BOOK_ID, B.BOOK_NAME, B.IMG
                    order by B.BOOK_ID
                    offset @Skip rows
                    fetch next @Take rows only
                    """;

            }
            else if(search != "" && category == "")
            {
                sql = """
                    select BOOK_ID as id, BOOK_NAME as name, IMG as image, count(*) over() as totalItems
                    from BOOK
                    where BOOK_NAME like @Search
                    order by BOOK_ID
                    offset @Skip rows
                    fetch next @Take rows only
                    """;
            }
            else if(search == "" && category != "")
            {
                sql = """
                    select B.BOOK_ID as id, B.BOOK_NAME as name, B.IMG as image, count(*) over() as totalItems
                    from BOOK as B 
                    join BOOK_CATEGORY as BC on B.BOOK_ID = BC.BOOK_ID
                    join CATEGORY as C on C.CATEGORY_ID = BC.CATEGORY_ID
                    where C.CATEGORY_NAME = @Category
                    group by B.BOOK_ID, B.BOOK_NAME, B.IMG
                    order by B.BOOK_ID
                    offset @Skip rows
                    fetch next @Take rows only
                    """;
            }
            if (search == "" && category == "")
            {
                sql = """
                    select BOOK_ID as id, BOOK_NAME as name, IMG as image, count(*) over() as totalItems
                    from BOOK
                    order by BOOK_ID
                    offset @Skip rows
                    fetch next @Take rows only
                    """;
            }

            int skip = (page - 1) * itemsPerPage;
            int take = itemsPerPage;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("@Skip", System.Data.SqlDbType.Int).Value = skip;
            command.Parameters.Add("@Take", System.Data.SqlDbType.Int).Value = take;
            command.Parameters.Add("@Search", System.Data.SqlDbType.NVarChar).Value = "%" + search + "%";
            command.Parameters.Add("@Category", System.Data.SqlDbType.NVarChar).Value = category;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (totalItems == 0)
                    {
                        totalItems = (int)reader["totalItems"];
                        totalItems = (totalItems < 0) ? 0 : totalItems;
                        totalPages = (totalItems / itemsPerPage) + (((totalItems % itemsPerPage) == 0) ? 0 : 1);
                        totalPages = (totalPages < 0) ? 0 : totalPages;
                    }
                    string bookId = (reader["id"] == DBNull.Value) ? "" : (string)reader["id"];
                    string bookName = (reader["name"] == DBNull.Value) ? "" : (string)reader["name"];
                    string image = (reader["image"] == DBNull.Value) ? "" : (string)reader["image"];
                    result.Add(new BookModel(bookId, bookName, image));
                }
            }
            return new Tuple<BindingList<BookModel>, int, int>(result, totalItems, totalPages);
        }

        public BookModel getBookDetail(string id)
        {
            BookModel result = new();
            string sql = """
                select * from BOOK
                where BOOK_ID = @Id
                """;
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("@Id", System.Data.SqlDbType.VarChar).Value = id;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string bookId = (reader["BOOK_ID"] == DBNull.Value) ? "" : (string)reader["BOOK_ID"];
                    string bookName = (reader["BOOK_NAME"] == DBNull.Value) ? "" : (string)reader["BOOK_NAME"];
                    int price = (reader["PRICE"] == DBNull.Value) ? 0 : (int)reader["PRICE"];
                    string author = (reader["AUTHOR"] == DBNull.Value) ? "" : (string)reader["AUTHOR"];
                    string image = (reader["IMG"] == DBNull.Value) ? "" : (string)reader["IMG"];
                    result.BookID = bookId; result.BookName = bookName;
                    result.Author = author; result.Image = image;
                    result.Price = price;
                }
            }
            return result;
        }
    }
}
