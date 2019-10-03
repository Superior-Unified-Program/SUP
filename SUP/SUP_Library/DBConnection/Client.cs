using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP_Library.DBConnection
{
    public class Client
    {
        private int    ID;
        private string Prefix;
        private string Company;
        private string Permit;
        private string PhoneNumber;
        private string EmailAddress;
        private string FirstName;
        private string LastName;
        private string MiddleInitial; // Shouldn't this be a char? Following the design documet
        private bool   Active;
        private int    Assistant;
        private string Notes;

        public Client(int ID)
        {
            setID(ID);
        }
        public Client(int ID, string FirstName, string LastName)
        {
            setID(ID);
            setFirstName(FirstName);
            setLastName(LastName);
        }
        public Client(int ID, string Prefix, string FirstName, string LastName)
        {
            setID(ID);
            setFirstName(FirstName);
            setLastName(LastName);
            setPrefix(Prefix);
        }
        public Client(int ID, string Prefix, string FirstName, string MiddleInitial, string LastName)
        {
            setID(ID);
            setFirstName(FirstName);
            setLastName(LastName);
            setPrefix(Prefix);
            setMiddleInitial(MiddleInitial);
        }
        public Client(int ID, string Prefix, string FirstName, string MiddleInitial, string LastName, string Company)
        {
            setID(ID);
            setFirstName(FirstName);
            setLastName(LastName);
            setPrefix(Prefix);
            setMiddleInitial(MiddleInitial);
            setCompany(Company);
        }
        public Client(int ID, string Prefix, string FirstName, string MiddleInitial, string LastName, string Company, string PhoneNumber)
        {
            setID(ID);
            setFirstName(FirstName);
            setLastName(LastName);
            setPrefix(Prefix);
            setMiddleInitial(MiddleInitial);
            setCompany(Company);
            setPhoneNumber(PhoneNumber);
        }
        public Client(int ID, string Prefix, string FirstName, string MiddleInitial, string LastName, string Company, string PhoneNumber, string EmailAddress)
        {
            setID(ID);
            setFirstName(FirstName);
            setLastName(LastName);
            setPrefix(Prefix);
            setMiddleInitial(MiddleInitial);
            setCompany(Company);
            setPhoneNumber(PhoneNumber);
            setEmailAddress(EmailAddress);
        }

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
        public int getID()
        {
            return ID;
        }
        private void setID(int ID)
        {
            // This method is private, since ID is the key, you shouldn't be able to change a clients's ID once it has been set.
            this.ID = ID;
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
    }
}
