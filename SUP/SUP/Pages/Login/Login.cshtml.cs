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

		//[HttpPOST]
        public string Login(string Username)
        {
			// return DatabaseConnection.verifiedLogIn(Username, Password);
			return "this";
        }

		public static string Test()
		{
			return "return string";
		}
    }
}
