using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP_Library
{
    public class DatabaseConnection
    {
        private static DatabaseConnection Instance;
        private DatabaseConnection() { }
        public static DatabaseConnection GetConnection()
        {
            if (Instance == null)
                Instance = new DatabaseConnection();

            return Instance;
        }

        public Boolean Login(string Username, string Password)
        {
            return (Username == "TEST");
        }
    }


}
