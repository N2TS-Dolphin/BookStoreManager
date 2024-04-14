using BookStoreManager.Database;
using BookStoreManager.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.Process
{
    public class CustomerManagerBus
    {
        public CustomerDao Dao { get; set; }
        public CustomerManagerBus() { 
            Dao = new CustomerDao();
        }
        public void AddCustomer(CustomerModel customer)
        {
            Dao.InsertCustomerIntoDB(customer);
        }

        public void UpdateCustomer(CustomerModel customer) 
        {
            Dao.UpdateCustomerToDB(customer);
        }

        public void DeleteCustomer(CustomerModel customer)
        {
            Dao.DeleteCustomerFromDB(customer);
        }
    }
}
