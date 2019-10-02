using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SUP_Library;

namespace SUP.Pages.Login
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
        }

        public static Boolean Login(string Username, string Password)
        {
            SUP_Library.DatabaseConnection conn = SUP_Library.DatabaseConnection.GetConnection();
            return conn.Login(Username, Password);
        }
    }
}
