using BookStoreManager.Database;
using BookStoreManager.DataType;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.Process
{
    public class CustomerBus
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Search { get; set; }
        public CustomerDao Dao { get; set; }
        public CustomerBus() {
            CurrentPage = 1;
            TotalPages = 0;
            Dao = new CustomerDao();
            Search = "";
        }
        public Tuple<BindingList<CustomerModel>, int, int> GetCustomerList()
        {
            var (items, totalPages) = Dao.GetCustomerListFromDB(CurrentPage, 9, Search);
            TotalPages = totalPages;
            CurrentPage = (TotalPages <= 0) ? 0 : CurrentPage;
            return new Tuple<BindingList<CustomerModel>, int, int>(items, TotalPages, CurrentPage);
        }
        public void MoveToPreviousPage()
        {
            CurrentPage = (CurrentPage-- <= 1) ? 1 : CurrentPage;
        }
        public void MoveToNextPage()
        {
            CurrentPage = (CurrentPage++ >= TotalPages) ? TotalPages : CurrentPage;
        }
        public void SearchCustomer(string search)
        {
            Search = search;
            CurrentPage = 1;
        }
        public void RefreshPage() { 
            CurrentPage = 1; 
            Search = "";
        }
    }
}
