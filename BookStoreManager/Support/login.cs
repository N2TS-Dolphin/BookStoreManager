using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using BookStoreManager.Database;
using System.Security.Cryptography.X509Certificates;

namespace BookStoreManager.Support
{
    public class Login
    {
        private static Login? _instance;
        private int _index;

        public static Login Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Login();
                return _instance;
            }
        }

        private Login() { }

        public int Get()
        {
            return _index;
        }

        public void Set(int index)
        {
            _index = index;
        }
    }
}
