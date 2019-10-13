using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SUP_Library;

namespace SUP_MVC.Controllers
{
    public class AddClientController : Controller
    {
        // GET: AddClient
        public ActionResult Index()
        {
            return View();
        }

        // GET: AddClient/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AddClient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AddClient/Create
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

        // GET: AddClient/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AddClient/Edit/5
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

        // GET: AddClient/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AddClient/Delete/5
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

        // POST: Search/SearchClients
        [HttpPost]
        public string GetClient([FromBody] string args)
        {
            try
            {
                var clientId = args;
                // NOTE: The below line of code will be added once the "GetClientById" functionality is implemented.
                var Clients = DatabaseConnection.GetClientById(clientId);
                var json = JsonConvert.SerializeObject(Clients);

                return json;
            }
            catch (Exception e)
            {
                return "FAAAAAILLL";
            }
        }

        public ActionResult AddClient()
		{
			return View();
		}
	}
}