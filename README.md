# SUP
Welcome to the SUP project.
To access the current version of the website you can use the following link:

sup.simple-url.com

NOTE: This url may be subject to change. 

In order for the database to be implemented the database script(DatabaseSqlCode.SQL.txt), within the GitHub project files, needs to be executed. SSMS (SQL Server Management Studio) will also needed to be installed. Then you will need to start SSMS and login using localhost as the server with windows authentication and if using a server that is already set up then you would need the login information for that server. Once logged in, then create a new query by clicking the "New Query" button on the toolbar and copy and paste the contents of DatabaseSqlCode.SQL.txt into the new query window and execute. There should be no issues as long as the compatibility level is at least 120 and this can be obtained using the following select statement, "SELECT compatibility_level FROM sys.databases WHERE name = 'DatabaseName'". In that command you would just need to replace 'DatabaseName' with the name of the database you are currently working in. If your compatibility level is lower than 120 (version 12) then the database script may need to be altered in order to run properly.

As a shortened version here are the steps described prior:

If installing a fresh version of SQL server:

  1. Install MS SQL Server.
  2. Install SSMS.
  3. Login to desired SQL server.
  4. Create a new query window.
  5. Copy and paste script from the DatabaseSqlCode.SQL.txt file.
  6. Execute the script.
  
If using a previously created version of SQL server:

  1. Login to desired SQL server.
  2. Create a new query window.
  3. Create a new query window.
  4. Copy and paste script from the DatabaseSqlCode.SQL.txt file.
  5. Execute the script.

NOTE: This script creates a user that has limited access to the database tables and stored procedures. It doesn't have a secure password and would need to be changed in a real implementation of the database and this change would also require a change in the SQL connection string in the middleware.

NOTE: There is a stored procedure to add dummy data called addDummyData. Go to the server in the object explorer and then drop down Databases > databaseName(your database's name) >  programability > Stored Procedures and you should see a list of stored procedures if you've completed the steps listed above. Find "addDummyData" and right click it and then click "Execute Stored Procedure...".

NOTE: The DatabaseSqlCode.SQL.txt script adds in a default login and a new login should be created and this initial one should be deleted. The login information for this default login is username = "admin" and password = "password". Failure to remove this is a potential security risk but is needed to initially get into the system since there is no way to add a login from the entry point of the system.

In Visual Studio:

Change  the value of "SupConnectionString" in appsettings.json to match the IP address and authentication of the database that you just set up.

Set the start up project to SUP-MVC by right-clicking in the solution and selecting "Set as StartUp Project."

To be able to test the Merge feature in this release:

This template relies on the client's address and parking pass number, so make sure for optimal results to test out on clients with this information filled out.

To perform merge, select clients to perform merge on, hit "Select Template", and choose "Guest_Parking_Letter_Template.docx." Hit "Continue" followed by "Generate Output." A zip file with the merged documents will be presented for download.
