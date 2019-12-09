using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP_Library.DBComponent
{
    public class EmailAddress
    {
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
        public EmailAddress()
        {
            /* Initialize string values with empty strings
             * to prevent issues with null values. It seems to work better when working across technologies to
             * use empty strings instead of trying to use nulls
             */
            Personal_Email = Assistant_Email = Business_Email = "";   
        }
                   
    }
}
