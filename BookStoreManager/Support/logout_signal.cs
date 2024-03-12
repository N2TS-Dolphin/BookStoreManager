using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManager.Support
{
    internal class logout_signal
    {
        private bool _signal = false;

        private static logout_signal _instance;
        public static logout_signal Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new logout_signal();
                return _instance;
            }
        }

        public bool Get() { return _signal; }
        public void Set(bool value) { _signal = value; }
    }
}
