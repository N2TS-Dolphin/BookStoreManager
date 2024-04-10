using BookStoreManager.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.Process
{
    public class OrderBus
    {
        private OrderDao orderDao = new OrderDao();

        public BindingList<OrderModel> GetAllOrders()
        {
            return orderDao.GetAllOrdersFromDB();
        }

        public Tuple<BindingList<OrderModel>, int, int> GetAllPaging(int page, int rowsPerPage, DateTime? fromDate, DateTime? toDate)
        {
            return orderDao.GetAllPagingFromDB(page, rowsPerPage, fromDate, toDate);
        }

        public void DeleteOrder(int orderId)
        {
            orderDao.DeleteOrderFromDB(orderId);
        }

        public void AddOrder(string customerName, DateTime orderDate)
        {
            orderDao.AddOrderToDB(customerName, orderDate);
        }

        public OrderModel GetOrderById(int orderId)
        {
            return orderDao.GetOrderByIdFromDB(orderId);
        }

        public void UpdateOrder(int orderId, string newCustomerName, DateTime newOrderDate, int newPrice)
        {
            orderDao.UpdateOrderToDB(orderId, newCustomerName, newOrderDate, newPrice);
        }
    }
}
