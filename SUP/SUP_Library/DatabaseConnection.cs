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
using System.Security.Cryptography;

namespace SUP_Library
{
    public class DatabaseConnection
    {
		static string publicKey = "MIGeMA0GCSqGSIb3DQEBAQUAA4GMADCBiAKBgHZ8jooMFeyy2Mhyybd6HGIel7525kI06eKd9rVCdo7hSU/lDPGDm5FO4+YTKFCi92E0fMhaZgLVlC0qr7NRNG/ieLbiw8hCx2LMLerW4wEGpDs3brJVraxx+W4kakyhm4YID2+cmw+xzo34+ZSu+7k12QCumHaJD+iqy1AMQBWTAgMBAAE=";
		private static Byte[] privateKey = Convert.FromBase64String("MIICWgIBAAKBgHZ8jooMFeyy2Mhyybd6HGIel7525kI06eKd9rVCdo7hSU/lDPGDm5FO4+YTKFCi92E0fMhaZgLVlC0qr7NRNG/ieLbiw8hCx2LMLerW4wEGpDs3brJVraxx+W4kakyhm4YID2+cmw+xzo34+ZSu+7k12QCumHaJD+iqy1AMQBWTAgMBAAECgYAET9ocGf5+Q6/x84N1wuLfiz94dflBNY3BaoA87nNEFdzGJI7JB0IVEqrmh5HzBUs9ZVyZKfkGZ2FiF2iXfQAMeTpIzWsYw9ronS3yWUCQruSyqlyex1jizsP3k7Yd/RBAFJo/iJCNUsAkhfI9IrD4yy5x/dCYpoYei6ViCfWeOQJBAOB1/wSau+RKfkISClXwaK5NCEjXso9wK0KA02S9xk2OtPgEV44/C2cRLyWPinIoCe8g1Ajmoh/rmyQKLYCXpycCQQCHIpgguG57Ex1rPisU+a+yCKzca0wzwrGhz0vj4Sb+QwGzzhyVmHW3GcI1+cGT3bjrkHDi0F74dk0fSeqxekG1AkBcnTMxEitOodIArvLmzMBUkuJFNAKwHocq9H7ExWzqGWTgJOJ/hdHNoBCE/foQ6iZXLYNvfMIOS6eCslReB7TnAkBhcca1QYkZYq1CGfBDDdFt1eegghbO9EPW5H5a8o6FppfhqmzeSrQHtqFe/pxiHe4sn1lnlM4G6HewakK8e+ZJAkBGUJV1JU1md3JxfHQIRNOf9imB8zuoks9m1Oeih/Vn9up7k68oGSm+jGP7HeUJWcpFDlBoMmZljgIPL72Nuh7l");
		public static Byte[] getPrivateKey()
		{
			return privateKey;
		}
		public static string getPublicKey()
		{
			return publicKey;
		}


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
        public static string verifiedLogIn(string theUsername, string thePassword)
        {
            
            try
            {
               
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {
                    var sql = "validateLogin";
                    var par = new DynamicParameters();
                    par.Add("@userName", theUsername);
                    par.Add("@givenPW", thePassword);
                    par.Add("Result", 0, direction: ParameterDirection.ReturnValue);
                   

                    var data = connection.Query<UserReturn>(sql,
                               new { @userName = theUsername, @givenPW = thePassword }, commandType: CommandType.StoredProcedure).ToList();
                    var resultingUser = data.FirstOrDefault();
                   
                    if (resultingUser.Account_Locked)
                        return "locked";
                    else if (!resultingUser.Valid_Login)
                        return "fail";
                    else if (resultingUser.Valid_Login)
                        return "success";
                    else
                        return "unknown";
                    
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

		public static bool addAccount(string theUsername, string thePassword, char userType, string theOffice)
		{
			try
			{
				using (IDbConnection connection = new SqlConnection(getConnectionString()))
				{
					string sql = "addAccount"; //name of stored procedure
					
					var par = new DynamicParameters();

					par.Add("@userName", theUsername);
					par.Add("@password", thePassword);
					par.Add("@userType", userType);
					par.Add("@office", theOffice);

					connection.Execute(sql, par, commandType: CommandType.StoredProcedure);
					return true;
				}
			}
			catch (Exception exc)
			{
				return false;
				System.Diagnostics.Debug.WriteLine(exc.Message);
				throw exc;
			}
		}
        public static bool deleteAccount(string userName) // this is very dangerous, proceed with caution!
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {
                    string sql = "deleteAccount"; //name of stored procedure
                                               
                    var par = new DynamicParameters();

                    par.Add("@user_name", userName); // name of the user to be deleted
                    par.Add("result", 0, direction: ParameterDirection.ReturnValue);

                    connection.Execute(sql, par, commandType: CommandType.StoredProcedure);

                    int intResult = par.Get<int>("result");

                    bool success = false;

                    if (intResult == 1) success = true;

                    return success;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public static bool isAdmin(string userName)
        {
            // takes a username and returns whether or not they are an administrator
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {
                    string sql; //name of stored procedure

                    sql = "checkType";

                    var par = new DynamicParameters();
                    
                    par.Add("@user_name", userName); 

                    par.Add("result", 0, direction: ParameterDirection.ReturnValue);

                    connection.Execute(sql, par, commandType: CommandType.StoredProcedure);
                    int intResult = par.Get<int>("result");
                    bool result = false;
                    if (intResult == 1) result = true;
                    return result;

                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
                throw exc;
            }
        }


        #endregion


        #region Client Connection

        // all connections related to Client table go here

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
                   
                    // Phone
                    par.Add("@Business_phoneNumber", client.Phone?.Business_Phone);
                    par.Add("@Personal_phoneNumber", client.Phone?.Personal_Phone);
                    // Email
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
					par.Add("@communityBreakfast", client?.Community_Breakfast);
					par.Add("@holidayCard", client?.Holiday_Card);
					
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
                            bool update = false;
                            
                            foreach(Organization org in orgs)
                            {
                                if (org.Org_Name == o.Org_Name && org.Org_Type == o.Org_Type)
                                {
                                    add = false;
                                    if (org.Title != o.Title || org.Primary != o.Primary) update = true;
                                }
                                
                                
                            }
                            if (add) addOrganization(client, o);
                            else if (update) updateOrganization(client, o);
                        }
                        
                        foreach (Organization org in orgs)
                        {
                            bool delete = true;
                            foreach (Organization o in client.Organizations)
                            {
                                if (org.Org_Name == o.Org_Name && org.Org_Type == o.Org_Type) delete = false;
                            }
                            if (delete) deleteOrganization(client.ID, org);
                        }
                    }

                    // get ID and return it
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
        public static void deleteClient(Client client)
        {
            deleteClient(client.ID);
        }
        public static void deleteClient(int clientID)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {
                    string sql = "deleteClient"; //name of stored procedure

                    // Parameters to send to addClient/updateClient stored proedure
                    var par = new DynamicParameters();

                    // organization parameters
                    par.Add("@ID", clientID);
               
                    connection.Execute(sql, par, commandType: CommandType.StoredProcedure);

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
        public static List<Client> QueryClientFull(Client client)
        {
            return QueryClientFull(client.Last_Name, client.First_Name, client.Primary_Organization.Org_Type, client.Primary_Organization.Title);
        }
        public static List<Client> QueryClientFull(string qLastName, string qFirstName, string qOrgType, string qTitle = "")
        {
            List<Client> data = null;
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {

                    var sql = "queryClientFull";   // name of stored procedure              

                    /* Send request for queryClient stored procedure with values for lastName, firstName and orgType provided
                     * Stored procedure joins four tables, so we have Dapper put first table values into Client class and into classes found within Client
                     * The joined tables are split at the Client_ID column
                     */

                    data = connection.Query<Client, Address, EmailAddress, PhoneNumber, Client>(sql, (client, address, email, phone) =>
                          {  client.Address = address; client.Email = email; client.Phone = phone; return client; },
                             new    { lastName = qLastName, firstName = qFirstName }, null, true, "Client_ID", commandType: CommandType.StoredProcedure).ToList();

                   
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

        #region Organization Connection

        // all connections related to Organization table go here

        public static void addOrganization(Client client, Organization organization)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {
                    string sql = "addOrganization"; //name of stored procedure

                    // Parameters to send to addOrganization stored proedure
                    var par = new DynamicParameters();

                    // organization parameters
                    par.Add("@clientID", client?.ID);
                    par.Add("@orgName", organization?.Org_Name);
                    par.Add("@orgType", organization?.Org_Type);
                    par.Add("@title", organization?.Title);
                    par.Add("@primary", organization?.Primary);

                    connection.Execute(sql, par, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
                throw exc;
            }
        }
        public static void updateOrganization(Client client, Organization organization)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {
                    string sql = "updateOrganization"; //name of stored procedure

                    // Parameters to send to updateOrganization stored proedure
                    var par = new DynamicParameters();

                    // organization parameters
                    par.Add("@id", client?.ID);
                    par.Add("@orgName", organization?.Org_Name);
                    par.Add("@orgType", organization?.Org_Type);
                    par.Add("@title", organization?.Title);
                    par.Add("@primary", organization?.Primary);

                    connection.Execute(sql, par, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
                throw exc;
            }
        }
        public static void deleteOrganization(int clientID, Organization organization)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(getConnectionString()))
                {
                    string sql = "deleteOrganization"; //name of stored procedure

                    // Parameters to send to updateOrganization stored proedure
                    var par = new DynamicParameters();

                    // organization parameters
                    par.Add("@ID", clientID);
                    par.Add("@org_name", organization?.Org_Name);
                    par.Add("@org_type", organization?.Org_Type);
                    
                    connection.Execute(sql, par, commandType: CommandType.StoredProcedure);

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
                                        
                    var data = connection.Query<Organization>(sql,
                               new { ID = clientID }, null, true, commandType: CommandType.StoredProcedure).ToList();

                    return data; // return list of organizations;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        #endregion

    }
}
