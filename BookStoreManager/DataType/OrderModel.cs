using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.DataType
{
    class OrderModel : INotifyPropertyChanged
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string OrderDate { get; set; }
        public int price { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
