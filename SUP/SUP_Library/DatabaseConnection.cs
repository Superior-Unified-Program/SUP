using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using SUP_Library.DBComponent;

namespace SUP_Library
{
    public class DatabaseConnection
    {
        #region Private Properties
        private static DatabaseConnection Instance;

        private static readonly string connectionStringName = "SUPDatabase";

        private DatabaseConnection() { }

        public static DatabaseConnection openConnection()
        {
            if (Instance == null) Instance = new DatabaseConnection();
            return Instance;
        }

        private static string getConnectionString()
        {
            // This retern the connection string in App.config file as the name same with connectionName.
            return "Initial Catalog=SUPdb; Data Source=68.112.175.122; Integrated Security=False; User Id=SUPuser; Password=abc123;";
            return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }
        #endregion

        #region Account Connection

        /// <summary>
        /// Check the username and harshed password with the database
        /// </summary>
        /// <param name="theUsername"></param>
        /// <param name="thePassword"></param> 
        /// <returns>
        /// true if both username and password are correct.
        /// false if one or both of them is wrong.
        /// </returns>
        public static bool verifiedLogIn(string theUsername, string thePassword)
        {
            thePassword = Harsh.HashPassword(thePassword);
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {
                    var currentAccount = connection.Query<Account>("dbo.SP_Account_RetrieveAccountByUsernameAndPassword @username @password", new { username = theUsername, password = thePassword }).ToList();
                    if (currentAccount.Count != 0)
                    {
                        if (thePassword != currentAccount[0].password) return false;
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public static bool verifiedLogInAdmin(string theUsername, string thePassword)
        {
            if(theUsername == "admin" || theUsername == "Admin")
            {
                if (thePassword == "password") return true;
                return false;
            }
            return false;
        }

        #endregion

        #region Address Connection

        // all connections related to Address table go here

        #endregion

        #region Client Connection

        // all connections related to Client table go here

        public static void addClient(Client newClient)
        {
            string conString = getConnectionString();

            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {

                    Console.WriteLine(connection.State);
                    
                    string sql = "addClient";
                    var affected = connection.Execute(sql,
                       new
                       {
                           firstName = newClient.First_Name,
                           lastName = newClient.Last_Name,
                           middleInitial = newClient.Middle_initial,
                           permitNum = newClient.Permit_Num,
                           active = newClient.Active,
                           notes = newClient.Notes
                       },
                        commandType: CommandType.StoredProcedure);
           
                }
            }
            catch (Exception exc)
            {
             
                throw exc;
            }
        }
        public static List<Client> QueryClient(string qLastName, string qFirstName, string qOrganization)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {
                   
                    var sql = "queryClient";   // name of stored procedure              

                    // Send request for queryClient stored procedure with values for lastName, firstName and orgType provided
                    // Stored procedure joins two tables, so we have Dapper put first table values into Client class and second into organization class found in client
                    // The joined table is split at the Client_ID column
                    var data = connection.Query<Client, Organization, Client>(sql, (client, org) => { client.Org = org; return client; }, 
                               new { lastName = qLastName, firstName = qFirstName, orgType = qOrganization },null,true,"Client_ID", commandType: CommandType.StoredProcedure).ToList();                                 

                    return data ; // return (client) list of results;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }


        }
        public static List<Client> QueryClientFull(string qLastName, string qFirstName, string qOrganization)
        {
            /* 
             * LEFT JOIN Works_For ON Client.ID = Works_For.Client_ID 
							LEFT JOIN Address ON Client.ID = Address.Client_ID
							LEFT JOIN Email ON Client.ID = Email.Client_ID
							LEFT JOIN Phone ON Client.ID = Phone.Client_ID
            */
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {

                    var sql = "queryClientFull";   // name of stored procedure              

                    // Send request for queryClient stored procedure with values for lastName, firstName and orgType provided
                    // Stored procedure joins five tables, so we have Dapper put first table values into Client class and into classes found within Client
                    // The joined tables are split at the Client_ID column
                    var data = connection.Query<Client, Organization,Address,EmailAddress,PhoneNumber, Client>(sql, (client, org,address,email,phone) => 
                               { client.Org = org; client.Address = address; client.Email = email; client.Phone = phone; return client; },
                               new { lastName = qLastName, firstName = qFirstName, orgType = qOrganization }, null, true, "Client_ID", commandType: CommandType.StoredProcedure).ToList();

                    return data; // return (client) list of results;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }


        }

        public static Client GetClientById(string clientId)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {
                    var sql = "getClientById";   // name of stored procedure

                    // Send request for getClientById stored procedure with values for Client_Id, the Id number for the client that should be retrieved
                    // Will retrieve a single client or a default value if results are empty
                    var data = connection.Query<Client>(sql, new { Client_Id = clientId }, commandType: CommandType.StoredProcedure).SingleOrDefault();

                    return data;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public static Client GetClientByIdFull(string clientId)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {
                    var sql = "getClientByIdFull";   // name of stored procedure

                    // Send request for getClientByIdFull. 

                    var data = connection.Query<Client, Organization, Address, EmailAddress, PhoneNumber, Client>(sql, (client, org, address, email, phone) =>
                    { client.Org = org; client.Address = address; client.Email = email; client.Phone = phone; return client; },
                               new { Client_ID = clientId }, null, true, "Client_ID", commandType: CommandType.StoredProcedure).SingleOrDefault();

                    //var data = connection.Query<Client>(sql, new { Client_Id = clientId }, commandType: CommandType.StoredProcedure).SingleOrDefault();

                    return data;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }


        }

        #endregion

        #region ContactInformation Connection

        // all connections related to ContactInformation table go here

        #endregion

        #region Organization Connection

        // all connections related to Organization table go here

        #endregion
    }
}