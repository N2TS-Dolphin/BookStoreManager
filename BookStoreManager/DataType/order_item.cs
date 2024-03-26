using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.DataType
{
    public class order_item
    {
        public string order_id { get; set; }
        public string book_id { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
    }
}
