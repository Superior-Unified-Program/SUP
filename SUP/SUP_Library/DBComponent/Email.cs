using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP_Library.DBComponent
{
    public class EmailAddress
    {
        
            public int Client_ID { get; set; }
            public string Personal_Email { get; set; }
            public string Assistant_Email { get; set; }

            public string Business_Email { get; set; }

            public string Email // deprecated 
            {
                get
                {
                    return Business_Email;
                }
                set
                {
                     Business_Email = value;
                }
            }
                   
    }
}
