using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager
{
    internal class OrderDetailModel
    {
        public int OrderID { get; set; }
        public BookModel Book { get; set; }
        public int Quantity { get; set; }
    }
}
