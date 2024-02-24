using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace BookStoreManager.Support
{
    public class accountInfo
    {
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
    }
}
