using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP_Library.DBComponent
{
    public class Address
    {
        public string Client_ID { get; set;  }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }


        public Address()
        {

        }
        public Address(string LineOne, string City, string State, string Zipcode) : this(LineOne,null,City,State,Zipcode)
        {
            // For now if we call constructor without Address LineTwo, we just pass on to the constructor with an empty LineTwo
        }

        public Address(string LineOne, string LineTwo, string City, string State, string Zipcode)
        {
            this.LineOne = LineOne;
            this.LineTwo = LineTwo;
            this.City = City;
            this.State = State;
            this.Zipcode = Zipcode;
            /* setLineOne(LineOne);
            setLineTwo(LineTwo);
            setCity(City);
            setState(State);
            setZipcode(Zipcode);
            */

        }
        /*
        public void setLineOne(string LineOne)
        {
            this.LineOne = LineOne;
        }

        public string getLineOne()
        {
            return LineOne;
        }
        public string getLineTwo()
        {
            return LineTwo;
        }
        public void setLineTwo(string LineTwo)
        {
            this.LineTwo = LineTwo;
        }
        public string getCity()
        {
            return City;
        }
        public void setCity(string City)
        {
            this.City = City;
        }
        public string getZipcode()
        {
            return Zipcode;
        }
        public void setZipcode(string Zipcode)
        {
            if (ValidZip(Zipcode))
                this.Zipcode = Zipcode;
            else
                throw new Exception("Invalid Zipcode!");
        }
        public string getState()
        {
            return State;
        }
        public void setState(string State)
        {
            this.State = State;
        }

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
