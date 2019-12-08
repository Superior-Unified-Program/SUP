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
        //public int Client_ID { get; set; }
        public string Org_Name { get; set; }
        public string Org_Type { get; set; }

        public string Title { get; set; }

        public bool Primary { get; set; }

       /* public Organization()
        {
            // initialize to empty values
            Client_ID = -1;
            Org_Name = "";
            Org_Type = "";
            Title = "";
            Primary = false;
        }*/
    }
}
