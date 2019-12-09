using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP_Library.DBComponent
{
    public class Organization
    {
        public string Org_Name { get; set; }

        public string Org_Type { get; set; }

        public string Title { get; set; }

        public bool Primary { get; set; }

        public Organization()
        {
           /* Initialize string values with empty strings
            * to prevent issues with null values. It seems to work better when working across technologies to
            * use empty strings instead of trying to use nulls
            */

            Org_Name = Org_Type = Title = "";
            Primary = false;
        }
    }
}
