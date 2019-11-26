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
            var appSettingsJson = ConnectionStringProvider.GetAppSettings();
            string connectionString = appSettingsJson["SupConnectionString"]; 
            // This retern the connection string in App.config file as the name same with connectionName.
            //return "Initial Catalog=SUPdb; Data Source=68.112.175.122; Integrated Security=False; User Id=SUPuser; Password=abc123;";
            //return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            return connectionString;
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
            //thePassword = Harsh.HashPassword(thePassword);
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {
                    var sql = "validateLogin";
                    var par = new DynamicParameters();
                    par.Add("@userName", theUsername);
                    par.Add("@givenPW", thePassword);
                    par.Add("Result", 0, direction: ParameterDirection.ReturnValue);
                    connection.Query(sql, par, commandType: CommandType.StoredProcedure);

                    var data = connection.Query<UserReturn>(sql,
                               new { @userName = theUsername, @givenPW = thePassword }, commandType: CommandType.StoredProcedure).ToList();
                    var resultingUser = data.FirstOrDefault();
                    var isValid = resultingUser.Valid_Login;
                    if (!isValid)
                    {
                        //TODO: Logic for incrementing user attempts, possibly lock user
                        resultingUser.Failed_Attempts++;
                        if (resultingUser.Failed_Attempts >= 3)
                        {
                            resultingUser.Account_Locked = true;
                        }
                    }
                    return isValid;
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



        /*
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
        */

        // addClient and updateClient are very similar, so these methods are being passed to a larger method called alterClient which changes a few things depending on which stored proc they are calling 
        public static int addClient(Client newClient)
        {
            return alterClient(newClient, true);
        }
        public static int updateClient(Client client)
        {
            return alterClient(client, false);
        }
        
        private static int alterClient(Client client, bool newClient)
        {
            // Fill out client class and pass to addClient/updateClient to add/update client and all associated data in database
            // Uses the database addClient or updateClient stored procedures
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {

                    // no support for assistant yet, until we figure out exactly how we are going to implement that
                    string sql; //name of stored procedure

                    if (newClient) // if newClient is true, then this method is being called from addClient
                        sql = "addClient";
                    else
                        sql = "updateClient";

                    // Parameters to send to addClient/updateClient stored proedure
                    var par = new DynamicParameters();



                    // Client base class parameters
                    if (!newClient)
                        par.Add("@ID", client.ID); // updateClient needs the client ID

                    par.Add("@prefix", client.Prefix);
                    par.Add("@firstName", client.First_Name);
                    par.Add("@lastName", client.Last_Name);
                    par.Add("@middleInitial", client.Middle_initial);
                    par.Add("@permitNum", client.Permit_Num);
                    par.Add("@active", client.Active);
                    par.Add("@notes", client.Notes);
                    // Organization Parameters
                   // par.Add("@orgName", client.Org?.Org_Name);
                   // par.Add("@orgType", client.Org?.Org_Type);
                   // par.Add("@title", client.Org?.Title);
                    // Phone
                    par.Add("@Business_phoneNumber", client.Phone?.Business_Phone);
                    par.Add("@Personal_phoneNumber", client.Phone?.Personal_Phone);
                    // Email
                    //par.Add("@email", client.Email.Email);
                    par.Add("@Business_email", client.Email?.Business_Email);
                    par.Add("@Personal_email", client.Email?.Personal_Email);
                    // Address
                    par.Add("@line1", client.Address.Line1);
                    par.Add("@line2", client.Address.Line2);
                    par.Add("@city", client.Address.City);
                    par.Add("@state", client.Address.State);
                    par.Add("@zipCode", client.Address.Zip);
                    // Assistant information
                    par.Add("@assistantFirstName", client?.Assistant_First_Name);
                    par.Add("@assistantLastName", client?.Assistant_Last_Name);
                    par.Add("@assistant_phoneNumber", client.Phone?.Assistant_Phone);
                    par.Add("@assistant_Email", client.Email?.Assistant_Email);
					// temp as stored procedure for update does not have these two fields
					//if (newClient)
					//{
						par.Add("@communityBreakfast", client?.Community_Breakfast);
						par.Add("@holidayCard", client?.Holiday_Card);
					//}
                    par.Add("result", 0, direction: ParameterDirection.ReturnValue);

                    connection.Execute(sql, par, commandType: CommandType.StoredProcedure);
                    int result = par.Get<int>("result");
                    
                    if (newClient)
                    {
                        client.ID = result;
                        foreach (Organization o in client.Organizations)
                            addOrganization(client,o);
                    }
                    else
                    {
                        List<Organization> orgs = queryOrganization(client.ID);
                        foreach (Organization o in client.Organizations)
                        {
                            bool add = true;
                            foreach(Organization org in orgs)
                            {
                                if (org.Org_Name == o.Org_Name && org.Org_Type == o.Org_Type) add = false;
                            }
                            if (add) addOrganization(client, o);
                        }
                    }

                    // get ID and return it
                    //int result = par.Get<int>("result");
                    System.Diagnostics.Debug.WriteLine("ID is: " + result);
                    return result;

                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
                throw exc;
            }
        }

        public static List<Client> checkForNearMatch(Client client)
        {
            return checkForNearMatch(client.Last_Name, client.First_Name);
        }

        public static List<Client> checkForNearMatch(string qLastName, string qFirstName)
        {
            // Implements the database stored procedure checkForNearMatch
            // Takes a first and last name and returns a list of Client objects with their IDs and names only filled out which are a near match
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {

                    var sql = "checkForNearMatch";   // name of stored procedure              

                    // Send request for checkForNearMatch stored procedure with values for lastName, firstName provided
                    var data = connection.Query<Client>(sql,
                               new { lastName = qLastName, firstName = qFirstName }, commandType: CommandType.StoredProcedure).ToList();

                    return data; // return (client) list of results;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public static void addOrganization(Client client, Organization organization)
        {
            // (clientID, orgName, orgType, title, primary)
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {
                    string sql="addOrganization"; //name of stored procedure

                    // Parameters to send to addClient/updateClient stored proedure
                    var par = new DynamicParameters();

                    // organization parameters
                    par.Add("@clientID", client?.ID);
                    par.Add("@orgName", organization?.Org_Name);
                    par.Add("@orgType", organization?.Org_Type);
                    par.Add("@title", organization?.Title);
                    par.Add("@primary", organization?.Primary);

                    connection.Execute(sql, par, commandType: CommandType.StoredProcedure);

                    // get ID and return it
                    //int result = par.Get<int>("result");
                    //System.Diagnostics.Debug.WriteLine("ID is: " + result);
                    //return result;

                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
                throw exc;
            }
        }
        public static List<Organization> queryOrganization(int clientID)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {

                    var sql = "queryOrganization";   // name of stored procedure              

                    // Send request for queryClient stored procedure with values for lastName, firstName and orgType provided
                    // Stored procedure joins two tables, so we have Dapper put first table values into Client class and second into organization class found in client
                    // The joined table is split at the Client_ID column
                    var data = connection.Query<Organization>(sql, 
                               new { ID = clientID }, null, true, commandType: CommandType.StoredProcedure).ToList();

                    return data; // return (client) list of results;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }
        public static List<Client> QueryClient(Client client)
        {
            return QueryClient(client.Last_Name, client.First_Name, client.Primary_Organization.Org_Type);
        }
        public static List<Client> QueryClient(string qLastName, string qFirstName, string qOrgType)
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
                               new { lastName = qLastName, firstName = qFirstName, orgType = qOrgType },null,true,"Client_ID", commandType: CommandType.StoredProcedure).ToList();                                 

                    return data ; // return (client) list of results;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }


        }

        public static List<Client> QueryClientFull(Client client)
        {
            return QueryClientFull(client.Last_Name, client.First_Name, client.Primary_Organization.Org_Type, client.Primary_Organization.Title);
        }
        public static List<Client> QueryClientFullOld2(string qLastName, string qFirstName, string qOrgType, string qTitle = "")
        {

            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {

                    var sql = "queryClientFull";   // name of stored procedure              

                    // Send request for queryClient stored procedure with values for lastName, firstName and orgType provided
                    // Stored procedure joins five tables, so we have Dapper put first table values into Client class and into classes found within Client
                    // The joined tables are split at the Client_ID column
                    var data = connection.Query<Client, Address, Organization, EmailAddress, PhoneNumber, Client>(sql, (client, address, organization, email, phone) =>
                    { client.Org = organization; client.Address = address; client.Email = email; client.Phone = phone; return client; },
                               new { lastName = qLastName, firstName = qFirstName, orgType = qOrgType, title = qTitle }, null, true, "Client_ID", commandType: CommandType.StoredProcedure).ToList();


                    System.Diagnostics.Debug.WriteLine("Client 0: " + data[0].Org.Org_Name + " " + data[0].Org.Org_Type + " " + data[0].Org.Title);

                    return data; // return (client) list of results;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }


        }
        public static List<Client> QueryClientFull(string qLastName, string qFirstName, string qOrgType, string qTitle = "")
        {
            List<Client> data = null;
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {

                    var sql = "queryClientFull";   // name of stored procedure              

                    // Send request for queryClient stored procedure with values for lastName, firstName and orgType provided
                    // Stored procedure joins five tables, so we have Dapper put first table values into Client class and into classes found within Client
                    // The joined tables are split at the Client_ID column
                    data = connection.Query<Client, Address, EmailAddress, PhoneNumber, Client>(sql, (client, address, email, phone) =>
                    {  client.Address = address; client.Email = email; client.Phone = phone; return client; },
                               new { lastName = qLastName, firstName = qFirstName }, null, true, "Client_ID", commandType: CommandType.StoredProcedure).ToList();

                    

                    //System.Diagnostics.Debug.WriteLine("Client 0: " + data[0].Org.Org_Name + " " + data[0].Org.Org_Type + " " + data[0].Org.Title);

                    //return data; // return (client) list of results;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            foreach (Client c in data)
            {
                c.Organizations = queryOrganization(c.ID);
            }
            return data;
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
                System.Diagnostics.Debug.WriteLine(exc.Message);
                throw exc;
            }
        }
        
        public static Client GetClientByIdFull(string clientId)
        {
            Client data = null;
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {
                    var sql = "getClientByIdFull";   // name of stored procedure

                    // Send request for getClientByIdFull. 

                    data = connection.Query<Client, Address, EmailAddress, PhoneNumber, Client>(sql, (client, address, email, phone) =>
                    {  client.Address = address; client.Email = email; client.Phone = phone; return client; },
                               new { Client_ID = clientId }, null, true, "Client_ID", commandType: CommandType.StoredProcedure).SingleOrDefault();

                    
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
                throw exc;
            }

            if (data!=null) data.Organizations = queryOrganization(data.ID);
            return data;
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
 