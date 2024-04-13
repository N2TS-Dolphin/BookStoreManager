using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.DataType
{
    public class CustomerModel : INotifyPropertyChanged, ICloneable
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public CustomerModel() {
            CustomerID = 0;
            CustomerName = string.Empty;
            CustomerEmail = string.Empty;
            CustomerPhone = string.Empty;
        }
        public CustomerModel(int customerID, string customerName, string customerEmail, string customerPhone)
        {
            CustomerID = customerID;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            CustomerPhone = customerPhone;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
