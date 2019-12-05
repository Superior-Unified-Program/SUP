﻿using System;
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

                //TODO: HASH HERE
                var hashedBytes = GetHash(password);
				var hashedPassword = Encoding.UTF8.GetString(hashedBytes, 0, hashedBytes.Length).Replace("'","");
				var LoginSuccessful = DatabaseConnection.verifiedLogIn(userName, hashedPassword);
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
			return View();
		}
	}
}