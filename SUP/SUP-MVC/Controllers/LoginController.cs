using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SUP_Library;
using Microsoft.AspNetCore.Session;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.OpenSsl;


using Org.BouncyCastle.Security;

namespace SUP_MVC.Controllers
{
	public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
           
        }

        // GET: Login/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Login/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
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

        // GET: Login/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Login/Edit/5
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

        // GET: Login/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Login/Delete/5
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
		
		[HttpPost]
        public string AuthenticateUser([FromBody] string args)
        {
            try
            {
                string[] separatedArgs = args.Split(',');
                if (separatedArgs.Length != 2)
                {
                    throw (new Exception("Oopsie"));
                }
                var userName = separatedArgs[0];
                var password = separatedArgs[1];
				
				ReadOnlySpan<byte> pkBytes = new ReadOnlySpan<byte>(SUP_Library.DatabaseConnection.getPrivateKey());
				RSACryptoServiceProvider p = new RSACryptoServiceProvider();
				p.ImportRSAPrivateKey(new ReadOnlySpan<byte>(SUP_Library.DatabaseConnection.getPrivateKey()), out int bytesRead);
				string decryptedPassword = CustomRSA.Decrypt(p, password );
				
				//TODO: HASH HERE
				var LoginSuccessful = DatabaseConnection.verifiedLogIn(userName, decryptedPassword);
                if (LoginSuccessful)
                {
                    //TODO: STORE SESSION HERE
                    TempData["UserID"] = userName;
                    TempData["LoginDate"] = DateTime.Now.ToShortDateString();
                    TempData["LoginTime"] = DateTime.Now.ToShortTimeString();
                }
                // if searching for active clients only, remove inactive clients.
                var json = JsonConvert.SerializeObject(LoginSuccessful);

                return json;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ActionResult Login()
		{
            TempData["UserID"] = null;
			return View();
		}

		[HttpPost]
		public string GetPublicKey()
		{
			return SUP_Library.DatabaseConnection.getPublicKey();
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
	}
}