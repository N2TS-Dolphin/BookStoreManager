using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreManager.Database;
using System.Security.Cryptography.X509Certificates;

namespace BookStoreManager.DataType
{
    public class LoginState
    {
        private static LoginState? _instance;
        private int _index;

        public static LoginState Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LoginState();
                return _instance;
            }
        }

        private LoginState() { }

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
