using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace BookStoreManager.Support
{
    public class accountInfo
    {
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string entropy { get; set; }
        
    }
}
