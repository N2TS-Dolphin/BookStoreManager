using BookStoreManager.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.Process
{
    public class OrderDetailBus
    {
        private OrderDetailDao orderDetailDao;
        private OrderDao orderDao;
        private BookDao bookDao;

        public OrderDetailBus()
        {
            orderDetailDao = new OrderDetailDao();
            orderDao = new OrderDao();
            bookDao = new BookDao();
        }


        public BindingList<OrderDetailModel> GetOrderDetails(int orderId)
        {
            return orderDetailDao.GetOrderDetailsFromDB(orderId);
        }

        public void SaveNewOrderItems(int orderId, BindingList<OrderDetailModel> orderDetails)
        {
            orderDetailDao.DeleteOrderItemsFromDB(orderId); //delete existing items

            foreach (var orderDetail in orderDetails)
            {
                orderDetailDao.InsertOrderItemToDB(orderId, orderDetail);
            }
        }

        public void InsertOrderItem(int orderId, OrderDetailModel orderDetail)
        {
            orderDetailDao.InsertOrderItemToDB(orderId, orderDetail);
        }

        public void UpdateOrder(int orderId, int newCustomerId, DateTime newOrderDate, int newTotalPrice)
        {
            orderDao.UpdateOrderToDB(orderId, newCustomerId, newOrderDate, newTotalPrice);
        }

        public OrderModel GetOrderById(int orderId)
        {
            return orderDao.GetOrderByIdFromDB(orderId);
        }

        public BindingList<CategoryModel> GetAllCategory()
        {
            var list = CategoryDao.GetCategoryListFromDB();
            list.Insert(0, new CategoryModel(0, "All"));
            list.OrderBy(x => x.CategoryName).ToList();
            return list;
        }

        public BindingList<BookModel> GetBooksByCategory(int categoryId)
        {
            return BookDao.GetBooksByCategoryFromDB(categoryId);
        }

        public BookModel GetBookDetail(int id)
        {
            return BookDao.GetBookDetailFromDB(id);
        }

    }
}
