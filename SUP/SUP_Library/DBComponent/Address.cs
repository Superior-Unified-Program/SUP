using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP_Library.DBComponent
{
    public class Address
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public Address()
        {
            Line1 = Line2 = City = State = Zip = "";
        }
        public Address(string LineOne, string City, string State, string Zipcode) : this(LineOne, "", City, State, Zipcode) { }

        public Address(string LineOne, string LineTwo, string City, string State, string Zipcode)
        {
            Line1 = LineOne;
            Line2 = LineTwo;
            this.City = City;
            this.State = State;
            Zip = Zipcode;
        }
       
        /*
        public static bool ValidZip(string Zipcode)
        {
            // Will there be non-US addresses? 

            bool validZipcode = true;
            const int ZIP_LEN1 = 5;
            const int ZIP_LEN2 = 9;

            if (!Int32.TryParse(Zipcode, out int i))
            {
                validZipcode = false;
            }
            else if (Zipcode.Length != ZIP_LEN1 && Zipcode.Length != ZIP_LEN2)
            {
                validZipcode = false;
            }

            return validZipcode;

        }
        */
    }
}
