using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SUP_Library;
using SUP_MVC.Models.Search;

namespace SUP_MVC.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        // GET: Search/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Search/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Search/Create
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

        // GET: Search/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Search/Edit/5
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

        // GET: Search/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Search/Delete/5
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
        public string SearchClients([FromBody] string args)
        {
            try
            {
                string[] separatedArgs = args.Split(',');
                if (separatedArgs.Length != 4)
                {
                    throw(new Exception("Oopsie"));
                }

                var Clients = DatabaseConnection.QueryClientFull(separatedArgs[1], separatedArgs[0], separatedArgs[2]);

                // if searching for active clients only, remove inactive clients.
                if (separatedArgs[3] == "true")
                {
                    var clientCount = Clients.Count();
                    for (var i = 0; i < clientCount; i++)
                    {
                        var client = Clients.ElementAt(i);
                        if (!client.Active)
                        {
                            Clients.Remove(client);
                            clientCount-=1;
                            i -= 1;
                        }
                    }
                }

                var json = JsonConvert.SerializeObject(Clients);
                
                return json;
            }
            catch(Exception e)
            {
                return "FAAAAAILLL";
            }
        }

        // POST: Search/SearchClients
        [HttpPost]
        public string ExportSearch([FromBody] string args)
        {
            try
            {
                string[] separatedArgs = args.Split(' ');

                if (separatedArgs.Length <= 0)
                {
                    throw (new Exception("Oopsie"));
                }

                var clientArray = new List<SUP_Library.DBComponent.Client>();
                foreach(var ID in separatedArgs)
                {
                    var currentClient = DatabaseConnection.GetClientByIdFull(ID);
                    if (currentClient != null)
                        clientArray.Add(currentClient);
                }
                SUP_Library.ExportFile.CreateExcelFile(clientArray);

                //TODO: return value should describe whether or not the process worked to the client.
                //  The below line is meaningless until then.
                var json = JsonConvert.SerializeObject("");
                return json;
            }
            catch (Exception e)
            {
                return "FAAAAAILLL";
            }
        }
        public ActionResult Search()
		{

			return View();
		}
    }
}