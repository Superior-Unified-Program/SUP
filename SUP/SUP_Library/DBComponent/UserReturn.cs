using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP_Library.DBComponent
{
    class UserReturn
    {
        public UserReturn()
        {
            Valid_Login = false;
            Account_Locked = true;
            Failed_Attempts = 0;
        }

        public bool Valid_Login { get; set; }
        public bool Account_Locked { get; set; }
        public int Failed_Attempts { get; set; }
    }
}
