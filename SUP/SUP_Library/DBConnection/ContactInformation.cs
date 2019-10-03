using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP_Library.DBConnection
{
    public class ContactInformation
    {
        private string Type;
        private string Value;

        public ContactInformation(string Type, string Value)
        {
            // this.Type = Type;
            // this.Value = Value;
            setType(Type);
            setValue(Value);
        }
        public string getType()
        {
            return Type;
        }
        public void setType(string Type)
        {
            this.Type = Type;
        }
        public string getValue()
        {
            return Value;
        }
        public void setValue(string Value)
        {
            this.Value = Value;
        }
    }
}
