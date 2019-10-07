using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP_Library
{
    public class DatabaseConnection
    {
        #region Private Connection

        private static DatabaseConnection Instance;
        private DatabaseConnection()
        {

        }
        public static DatabaseConnection getConnection()
        {
            if (Instance == null) Instance = new DatabaseConnection();
            return Instance;
        }

        #endregion
        
        #region 
        #endregion
    }


}

/*      List of procedure:
 *      
 * 
 */