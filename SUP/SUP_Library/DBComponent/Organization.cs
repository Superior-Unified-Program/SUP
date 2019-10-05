using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP_Library.DBComponent
{
    public class Organization
    {
        private string Type;
        private string Name;

        public Organization(string Type, string Name)
        {
           // this.Type = Type;
           // this.Name = Name;
            setType(Type);
            setName(Name);
        }
        public string getType()
        {
            return Type;
        }
        public void setType(string Type)
        {
            this.Type = Type;
        }
        public string getName()
        {
            return Name;
        }
        public void setName(string Name)
        {
            this.Name = Name;
        }
    }
}
