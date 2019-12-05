using System;
using System.Collections.Generic;
using System.Linq;
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
            return View();
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
            return View();
        }

        [HttpPost]
        public string SubmitUserData([FromBody] string args)
        {
            string[] separatedArgs = args.Split(',');
            if (separatedArgs.Length < 2)
            {
                throw (new Exception("Illegal number of arguments"));
            }
            var username = separatedArgs[0];
            var password = separatedArgs[1];

            //var listOfPossibleMatches = SUP_Library.DatabaseConnection.addUser(username, password);
            var result = true;
            var json = JsonConvert.SerializeObject(result);

            return json;
        }
    }
}
