using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP_Library.DBComponent
{
    public class PhoneNumber
    {
       // public int Client_ID { get; set; }

        public string Personal_Phone { get; set; }

        public string Business_Phone { get; set; }

        public string Assistant_Phone { get; set; }
        public string Number // deprecated
        { 
            get
            {
                return Business_Phone;
            }
            set
            {
                Business_Phone = value;
            }
        
        }
    }
}
