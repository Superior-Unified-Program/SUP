# SUP
Welcome to the SUP project.
To access the current version of the website you can use the following link:

sup.simple-url.com

NOTE: This url may be subject to change. 

In order for the database to be setup the database script within the project files needs to be executed after MS SQL Server is installed. SSMS (SQL Server Management Studio) will also needed to be installed. Then you will need to start SSMS and login using localhost as the server with windows authentication unless another server is desired and you would need the login for that SQL server. Once logged in, then create a new query and copy and paste the database script into the new query window and execute. There should be no issues as long as the compatibility level is at least 120 and this can be obtained using the following select statement, "SELECT compatibility_level FROM sys.databases WHERE name = 'DatabaseName'". In that command you would just need to replace 'DatabaseName' with the name of the database you are currently working in. If your compatibility level is lower than 120 then the database script may need to be altered in order to run properly.

As a shortened version here are the steps described prior:

  1. Install SQL server.
  2. Install SSMS.
  3. Login to desired SQL server.
  4. Create a new query window.
  5. Copy and paste script from GitHub from the create database script.
  6. Execute the script.

NOTE: This script creates a user that has limited access to the database tables and stored procedures. It doesn't have a secure password and would need to be changed in a real implementation of the database and this change would also require a change in the SQL connection string on the middleware.

There is a stored procedure to add multiple dummy data call addDummyData. Go to the server drop down the programmability and drop down stored procedures. Find addDummyData and right click on it and click execute.
