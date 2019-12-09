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
        private string _businessPhone;
        private string _personalPhone;
        private string _assistantPhone;
        public string Personal_Phone
        {
            get
            {
                return _personalPhone;
            }
            set
            {
                _personalPhone = removeFormatting(value);
            }
        }
        public string Business_Phone 
        { 
            get
            {
                return _businessPhone;
            }
            set
            {
                _businessPhone = removeFormatting(value);
            }
        }

        public string Assistant_Phone
        {
            get
            {
                return _assistantPhone;
            }
            set
            {
                _assistantPhone = removeFormatting(value);
            }
        }

        public string removeFormatting(string number)
        {
            if (number!=null)
                number = number.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();
            return number;
        }
        public string returnNumberWithFormatting(string number)
        {
            if (number.Length == 11)
            {
                number = number.Substring(0, 1) + " - (" + number.Substring(1, 3) + ") " + number.Substring(4, 3) + " - " + number.Substring(7, 4);
            }

            else if (number.Length == 10)
            {
                number = "(" + number.Substring(0, 3) + ") " + number.Substring(3, 3) + " - " + number.Substring(6, 4);
            }


            else if (number.Length == 7)
            {
                number = number.Substring(0, 3) + " - " + number.Substring(3, 4);
            }

            return number;
        }
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
        public PhoneNumber()
        {
            _businessPhone = "";
            _personalPhone = "";
            _assistantPhone = ""; 
        }
    }
}
