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
		string publicKey = "MIGeMA0GCSqGSIb3DQEBAQUAA4GMADCBiAKBgHZ8jooMFeyy2Mhyybd6HGIel7525kI06eKd9rVCdo7hSU/lDPGDm5FO4+YTKFCi92E0fMhaZgLVlC0qr7NRNG/ieLbiw8hCx2LMLerW4wEGpDs3brJVraxx+W4kakyhm4YID2+cmw+xzo34+ZSu+7k12QCumHaJD+iqy1AMQBWTAgMBAAE=";
		private static Byte[] privateKey = Convert.FromBase64String("MIICWgIBAAKBgHZ8jooMFeyy2Mhyybd6HGIel7525kI06eKd9rVCdo7hSU/lDPGDm5FO4+YTKFCi92E0fMhaZgLVlC0qr7NRNG/ieLbiw8hCx2LMLerW4wEGpDs3brJVraxx+W4kakyhm4YID2+cmw+xzo34+ZSu+7k12QCumHaJD+iqy1AMQBWTAgMBAAECgYAET9ocGf5+Q6/x84N1wuLfiz94dflBNY3BaoA87nNEFdzGJI7JB0IVEqrmh5HzBUs9ZVyZKfkGZ2FiF2iXfQAMeTpIzWsYw9ronS3yWUCQruSyqlyex1jizsP3k7Yd/RBAFJo/iJCNUsAkhfI9IrD4yy5x/dCYpoYei6ViCfWeOQJBAOB1/wSau+RKfkISClXwaK5NCEjXso9wK0KA02S9xk2OtPgEV44/C2cRLyWPinIoCe8g1Ajmoh/rmyQKLYCXpycCQQCHIpgguG57Ex1rPisU+a+yCKzca0wzwrGhz0vj4Sb+QwGzzhyVmHW3GcI1+cGT3bjrkHDi0F74dk0fSeqxekG1AkBcnTMxEitOodIArvLmzMBUkuJFNAKwHocq9H7ExWzqGWTgJOJ/hdHNoBCE/foQ6iZXLYNvfMIOS6eCslReB7TnAkBhcca1QYkZYq1CGfBDDdFt1eegghbO9EPW5H5a8o6FppfhqmzeSrQHtqFe/pxiHe4sn1lnlM4G6HewakK8e+ZJAkBGUJV1JU1md3JxfHQIRNOf9imB8zuoks9m1Oeih/Vn9up7k68oGSm+jGP7HeUJWcpFDlBoMmZljgIPL72Nuh7l");

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

		public static byte[] GetHash(string inputString)
		{
			HashAlgorithm algorithm = SHA256.Create();
			return algorithm.ComputeHash(System.Text.Encoding.UTF8.GetBytes(inputString));
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

				ReadOnlySpan<byte> pkBytes = new ReadOnlySpan<byte>(privateKey);
				RSACryptoServiceProvider p = new RSACryptoServiceProvider();
				p.ImportRSAPrivateKey(new ReadOnlySpan<byte>(privateKey), out int bytesRead);
				string decryptedPassword = CustomRSA.Decrypt(p, password );

				//TODO: HASH HERE
				var hashedBytes = GetHash(password);
				var hashedPassword = Encoding.UTF8.GetString(hashedBytes, 0, hashedBytes.Length).Replace("'","");
				var LoginSuccessful = DatabaseConnection.verifiedLogIn(userName, password);
                if (LoginSuccessful)
                {
                    //TODO: STORE SESSION HERE
                    TempData["UserID"] = userName;
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
			return publicKey;
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