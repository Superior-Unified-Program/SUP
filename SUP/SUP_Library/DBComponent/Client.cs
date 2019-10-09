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
        //public string Prefix { get; set; }
        public string Company { get; set; }
        public string Permit_Num { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public char Middle_initial { get; set; }

        public Organization Org { get; set; }
        /*public string MiddleInitial {
            get
            {
                return middleInitial;
            }
            set
            {
                if (MiddleInitial.Length > 1) // Since MiddleInitial is a string, we are making sure that we aren't putting anything more than an initial in it
                    throw new Exception("Middle initial must be one letter.");

                this.middleInitial = value;
            }
        }*/
            // Shouldn't this be a char? Following the design documet
        public bool   Active { get; set; }
        public int    Assistant { get; set; }
        public string Notes { get; set; }

        public Client()
        {

        }
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
            /*
            setId(Id);
            setFirstName(FirstName);
            setLastName(LastName);
            */
        }
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
        /*
        public string getFirstName()
        {
            return FirstName;
        }
        public void setFirstName(string FirstName)
        {
            if (FirstName == "" || FirstName == null)
                throw new Exception("Client must have a first name.");

            this.FirstName = FirstName;
        }
        public string getLastName()
        {
            return LastName;
        }
        public void setLastName(string LastName)
        {
            if (LastName == "" || LastName == null)
                throw new Exception("Client must have a last name.");

            this.LastName = LastName;
        }
        public string getMiddleInitial()
        {
            return MiddleInitial;
        }
        public void setMiddleInitial(string MiddleInitial)
        {
            if (MiddleInitial.Length > 1) // Since MiddleInitial is a string, we are making sure that we aren't putting anything more than an initial in it
                throw new Exception("Middle initial must be one letter.");

            this.MiddleInitial = MiddleInitial;
        }

        public string getPrefix()
        {
            return Prefix;
        }
        public void setPrefix(string Prefix)
        {
            this.Prefix = Prefix;
        }
        public string getCompany()
        {
            return Company;
        }
        public void setCompany(string Company)
        {
            this.Company = Company;
        }
        public string getPermit()
        {
            return Permit;
        }
        public void setPermit(string Permit)
        {
            this.Permit = Permit;
        }
        public string getPhoneNumber()
        {
            return PhoneNumber;
        }
        public void setPhoneNumber(string PhoneNumber)
        {
            this.PhoneNumber = PhoneNumber;
        }
        public string getEmailAddress()
        {
            return EmailAddress;
        }
        public void setEmailAddress(string EmailAddress)
        {
            this.EmailAddress = EmailAddress;
        }

        public string getNotes()
        {
            return Notes;
        }
        public void setNotes(string Notes)
        {
            this.Notes = Notes;
        }
        public int getId()
        {
            return Id;
        }
        private void setId(int Id)
        {
            // This method is private, since Id is the key, you shouldn't be able to change a clients's Id once it has been set.
            this.Id = Id;
        }
        public int getAssistant()
        {
            return Assistant;
        }
        public void setAssistant(int Assistant)
        {
            this.Assistant = Assistant;
        }
        public bool getActive()
        {
            return Active;
        }
        public void setActive(bool Active)
        {
            this.Active = Active;
        }
        */
    }
}
