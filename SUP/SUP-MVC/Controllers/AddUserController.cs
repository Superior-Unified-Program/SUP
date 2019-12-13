using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SUP_Library;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SUP_MVC.Controllers
{
    public class AddUserController : Controller
    {
        public ActionResult Index()
        {
            if (TempData["UserID"] != null)
            {
                TempData["UserID"] = TempData["UserID"];
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        // GET: AddUser/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AddUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AddUser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AddUser/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AddUser/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AddUser/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AddUser/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AddUser()
        {
            if (TempData["LoginDate"] != null && TempData["LoginTime"] != null)
            {
                int minutesTillLogout = 10;

                DateTime loadedDateTime = DateTime.ParseExact(TempData["LoginDate"].ToString(), "d", null);
                DateTime loadedTime = DateTime.ParseExact(TempData["LoginTime"].ToString(), "t", null);
                if (loadedDateTime.ToShortDateString().Equals(DateTime.Now.ToShortDateString()))
                {
                    var currentTime = DateTime.ParseExact(DateTime.Now.ToShortTimeString(), "t", null);
                    var tooLate = loadedTime;
                    tooLate = tooLate.AddMinutes(minutesTillLogout);
                    if (currentTime.TimeOfDay > tooLate.TimeOfDay)
                    {
                        return RedirectToAction("Login", "Login");
                    }
                    else
                    {
                        ResetTimeout();
                    }
                }
            }

            if (TempData["UserID"] != null)
            {
                TempData["UserID"] = TempData["UserID"];
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public string SubmitUserData([FromBody] string args)
        {
            ResetTimeout();
            string[] separatedArgs = args.Split(',');
            if (separatedArgs.Length < 2)
            {
                throw (new Exception("Illegal number of arguments"));
            }
            var username = separatedArgs[0];
            var password = separatedArgs[1];
			ReadOnlySpan<byte> pkBytes = new ReadOnlySpan<byte>(SUP_Library.DatabaseConnection.getPrivateKey());
			RSACryptoServiceProvider p = new RSACryptoServiceProvider();
			p.ImportRSAPrivateKey(new ReadOnlySpan<byte>(SUP_Library.DatabaseConnection.getPrivateKey()), out int bytesRead);
			string decryptedPassword = CustomRSA.Decrypt(p, password);
			var accountType = separatedArgs[2];
            var office = separatedArgs[3];
            var result = SUP_Library.DatabaseConnection.addAccount(username, decryptedPassword, accountType[0],office);
            var json = JsonConvert.SerializeObject(result);

            return json;
        }

		class CustomRSA
		{
			public const bool OAEP_PADDING = true;
			/// <summary>
			/// PKCS1 padding is required for most encryption using JavaScript packages
			/// </summary>
			public const bool PKCS1_PADDING = false;

			public static string Encrypt(
				RSACryptoServiceProvider csp,
				string plaintext
			)
			{
				return Convert.ToBase64String(
					csp.Encrypt(
						Encoding.UTF8.GetBytes(plaintext),
						PKCS1_PADDING
					)
				);
			}

			public static string Decrypt(
				RSACryptoServiceProvider csp,
				string encrypted
			)
			{
				return Encoding.UTF8.GetString(
					csp.Decrypt(
						Convert.FromBase64String(encrypted),
						PKCS1_PADDING
					)
				);
			}

		}
        private void ResetTimeout()
        {
            TempData["LoginDate"] = DateTime.Now.ToShortDateString();
            TempData["LoginTime"] = DateTime.Now.ToShortTimeString();
        }
    }
}
