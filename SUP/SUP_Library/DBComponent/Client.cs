using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP_Library.DBComponent
{
    public class Client
    {
        public int ID { get; set; }

        public string Prefix { get; set; }
        //public string Company { get; set; }
        public string Permit_Num { get; set; }
        //public string PhoneNumber { get; set; }
        //public string EmailAddress { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public char Middle_initial { get; set; }

        public Organization Org { get; set; }
        public Address Address { get; set; }

        public EmailAddress Email { get; set; }

        public PhoneNumber Phone { get; set; }

        public bool   Active { get; set; }
        public int    Assistant { get; set; }
        public string Notes { get; set; }

        public Client()
        {
            Org = new Organization(); // classes probably should be initialized and not null
            Address = new Address();
            Email = new EmailAddress();
            Phone = new PhoneNumber();
        }
        
        
        /*
        public Client(int Id)
        {
         //   this.Id = Id;
            //setId(Id);
        }
        public Client(int Id, string FirstName, string LastName)
        {
           // this.Id = Id;
           // this.FirstName = FirstName;
           // this.LastName = LastName;
            
            setId(Id);
            setFirstName(FirstName);
            setLastName(LastName);
            
        }
        */
       /* public Client(int Id, string Prefix, string FirstName, string LastName)
        {
            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Prefix = Prefix;
            
            setId(Id);
            setFirstName(FirstName);
            setLastName(LastName);
            setPrefix(Prefix);
            
        }*/
        /*public Client(int Id, string Prefix, string FirstName, char MiddleInitial, string LastName)
        {
            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Prefix = Prefix;
            this.MiddleInitial = MiddleInitial;
            
            setId(Id);
            setFirstName(FirstName);
            setLastName(LastName);
            setPrefix(Prefix);
            setMiddleInitial(MiddleInitial);
        }*/
       /* public Client(int Id, string Prefix, string FirstName, char MiddleInitial, string LastName, string Company)
        {
            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Prefix = Prefix;
            this.MiddleInitial = MiddleInitial;
            this.Company = Company;
            
            setId(Id);
            setFirstName(FirstName);
            setLastName(LastName);
            setPrefix(Prefix);
            setMiddleInitial(MiddleInitial);
            setCompany(Company);
            
        }
        public Client(int Id, string Prefix, string FirstName, char MiddleInitial, string LastName, string Company, string PhoneNumber)
        {
            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Prefix = Prefix;
            this.MiddleInitial = MiddleInitial;
            this.Company = Company;
            this.PhoneNumber = PhoneNumber;
            
            setId(Id);
            setFirstName(FirstName);
            setLastName(LastName);
            setPrefix(Prefix);
            setMiddleInitial(MiddleInitial);
            setCompany(Company);
            setPhoneNumber(PhoneNumber);
            
        }
        public Client(int Id, string Prefix, string FirstName, char MiddleInitial, string LastName, string Company, string PhoneNumber, string EmailAddress)
        {
            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Prefix = Prefix;
            this.MiddleInitial = MiddleInitial;
            this.Company = Company;
            this.PhoneNumber = PhoneNumber;
            this.EmailAddress = EmailAddress;
            
            setId(Id);
            setFirstName(FirstName);
            setLastName(LastName);
            setPrefix(Prefix);
            setMiddleInitial(MiddleInitial);
            setCompany(Company);
            setPhoneNumber(PhoneNumber);
            setEmailAddress(EmailAddress);
            
        }*/
     }
}
