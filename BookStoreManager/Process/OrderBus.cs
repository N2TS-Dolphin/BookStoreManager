using BookStoreManager.Database;
using BookStoreManager.DataType;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
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
        private CustomerDao customerDao = new CustomerDao();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; } 

        public OrderBus()
        {
            CurrentPage = 1;
            TotalPages = 0;
            FromDate = null;
            ToDate = null;
        }
        public BindingList<OrderModel> GetAllOrders()
        {
            return orderDao.GetAllOrdersFromDB();
        }

        public Tuple<BindingList<OrderModel>, int, int, int> GetAllPaging(DateTime? fromDate, DateTime? toDate)
        {
            var (items, totalItems, totalPages) = orderDao.GetAllPagingFromDB(CurrentPage, 10, fromDate, toDate);
            TotalPages = totalPages;
            CurrentPage = (TotalPages <= 0) ? 1 : CurrentPage;
  
            return new Tuple<BindingList<OrderModel>, int, int, int>(items, totalItems, TotalPages, CurrentPage);
        }


        public void MoveToNextPage()
        {
            CurrentPage = (CurrentPage >= TotalPages) ? TotalPages : CurrentPage + 1;
        }

      
        public void MoveToPreviousPage()
        {
            CurrentPage = (CurrentPage <= 1) ? 1 : CurrentPage - 1;
        }

        public void DeleteOrder(int orderId)
        {
            orderDao.DeleteOrderFromDB(orderId);
        }

        public void AddOrder(int customerId, DateTime orderDate, string orderAddress)
        {
            orderDao.AddOrderToDB(customerId, orderDate,orderAddress);
        }

        public OrderModel GetOrderById(int orderId)
        {
            return orderDao.GetOrderByIdFromDB(orderId);
        }

        public void UpdateOrder(int orderId,  DateTime newOrderDate, int newPrice, string newAddress)
        {
            orderDao.UpdateOrderToDB(orderId, newOrderDate, newPrice, newAddress);
        }

        public BindingList<CustomerModel> GetAllCustomers(string search)
        {
            return customerDao.GetAllCustomersFromDB(search);
        }

        public CustomerModel GetCustomerDetail(int customerId)
        {
            CustomerDao customerDao = new CustomerDao();
            return customerDao.GetCustomerDetailFromDB(customerId);
        }

        public BindingList<CustomerModel> SearchCustomers(string search)
        {
            return customerDao.GetAllCustomersFromDB(search);
        }
    }
}
