﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP_Library.DBComponent
{
    public class ContactInformation
    {
        public string Type { get; set; }
        public string Value { get; set; }

        public ContactInformation(string Type, string Value)
        {
             this.Type = Type;
             this.Value = Value;
            /* setType(Type);
            setValue(Value); */
        }

        /*
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
        */
    }
}
