using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP_Library.DBComponent
{
    public class Organization
    {
       // System.Int32 Client_ID, System.String Org_Name, System.String Org_Type, System.String Title) is required for SUP_Library.DBComponent.Organization materialization'


        public int Client_ID { get; set; }
        public string Org_Name { get; set; }
        public string Org_Type { get; set; }

        public string Title { get; set; }

        /*public Organization(string Type, string Name)
        {
            this.Type = Type;
            this.Name = Name;
            
            setType(Type);
            setName(Name);
            
        }*/
        /*
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
        */
    }
}
