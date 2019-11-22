using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP_Library.DBComponent
{
    public class Client : IComparable
    {
        public enum SortBy { Last_Name, First_Name, Org_Title, Org_Name, Business_Email, Business_Phone };

        public static void setSortCriteria(SortBy sortByCriteria, bool ascending)
        {
            if (ascending) asc = 1;
            else asc = -1;
            sortBy = sortByCriteria;
        }
        private static SortBy sortBy;
        private static int asc;
        public int ID { get; set; }

        public string Prefix { get; set; }
        //public string Company { get; set; }
        public string Permit_Num { get; set; }
        //public string PhoneNumber { get; set; }
        //public string EmailAddress { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Middle_initial { get; set; }

        public Organization Org { get; set; }
        public Address Address { get; set; }

        public EmailAddress Email { get; set; }

        public PhoneNumber Phone { get; set; }

        public bool Active { get; set; }

        public bool Holiday_Card { get; set; }

        public bool Community_Breakfast { get; set; }

        public string Assistant_First_Name { get; set; }
        public string Assistant_Last_Name { get; set; }
        public string Assisntant_Last_Name // allow dapper to pull in misspelled column in db
        { 
            get
            {
                return Assistant_Last_Name;
            }
            set
            {
                Assistant_Last_Name = value;
            }
        
        }
        public string Notes { get; set; }

        public Client()
        {
            Org = new Organization(); // classes probably should be initialized and not null
            Address = new Address();
            Email = new EmailAddress();
            Phone = new PhoneNumber();
            setSortCriteria(SortBy.Last_Name, true); // set to sort ascending and by last name by default
        }

        public int CompareTo(object client)
        {
            return CompareTo((Client)client);
        }
        private int CompareTo(Client client)
        {
            if (client == null) return asc * 1;
            switch (sortBy)
            {
                case SortBy.Org_Title:
                    if (client.Org.Title == null || client.Org.Title.Trim() == "") return asc * -1;
                    if (Org.Title == null || Org.Title.Trim() == "") return asc * 1;
                    if (Org.Title == client.Org.Title) return asc * Last_Name.CompareTo(Last_Name);
                    return asc * Org.Title.CompareTo(client.Org.Title);
                case SortBy.Org_Name:
                    if (client.Org.Org_Name == null) return asc * -1;
                    if (Org.Org_Name == null) return asc * 1;
                    if (Org.Org_Name == client.Org.Org_Name) return asc * Last_Name.CompareTo(Last_Name);
                    return asc * Org.Org_Name.CompareTo(client.Org.Org_Name);
                case SortBy.Business_Phone:
                    if (client.Phone.Business_Phone == null) return asc * -1;
                    if (Phone.Business_Phone == null) return asc * 1;
                    if (Phone.Business_Phone == client.Phone.Business_Phone) return asc * Last_Name.CompareTo(Last_Name);
                    return asc * Phone.Business_Phone.CompareTo(client.Phone.Business_Phone);
                case SortBy.Business_Email:
                    if (client.Email.Business_Email == null) return asc * -1;
                    if (Email.Business_Email == null) return asc * 1;
                    if (Email.Business_Email == client.Email.Business_Email) return asc * Last_Name.CompareTo(client.Last_Name);
                    return asc * Email.Business_Email.CompareTo(client.Email.Business_Email);
                case SortBy.First_Name:
                    if (client.First_Name == null) return asc * -1;
                    if (First_Name == null) return asc * 1;
                    if (First_Name == client.First_Name) return asc * this.Last_Name.CompareTo(client.Last_Name);
                    return asc * First_Name.CompareTo(client.First_Name);
                case SortBy.Last_Name:
                default:
                    if (client.Last_Name == null) return asc * -1; 
                    if (Last_Name == null) return asc * 1;
                    if (Last_Name == client.Last_Name) return asc * this.First_Name.CompareTo(client.First_Name);
                    return asc * Last_Name.CompareTo(client.Last_Name);
            }
        }
        public override string ToString()
        {
            return base.ToString() + ": " + ID.ToString() + " " + Prefix.ToString() + " " + Last_Name.ToString() + " " + First_Name.ToString() + " " + Middle_initial.ToString() + " " + Permit_Num.ToString() +
                   " " + Assistant_First_Name.ToString() + " " + Assistant_Last_Name.ToString() + " " + Active.ToString() + " " + Notes.ToString() + " " + Address.Line1.ToString() + " " +
                          Address.Line2.ToString() + " " + Address.City.ToString() + " " + Address.State.ToString() + " " + Address.Zip.ToString() + " " + Org.Org_Name + " " + Org.Org_Type + " " +
                          Org.Title + " P:" + Email.Personal_Email + " B:" + Email.Business_Email + " A:" + Email.Assistant_Email + " P:" + Phone.Personal_Phone + " B:" + Phone.Business_Phone + " A:" + Phone.Assistant_Phone;
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
