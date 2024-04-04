using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.DataType
{
    class RevenueModel : INotifyPropertyChanged
    {
        public DateTime OrderDate { get; set; }
        public int Revenue { get; set; }
        public int Quantity { get; set; }
        public int Month {  get; set; }
        public int Year { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
