using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace SUP_MVC.Controllers
{
    public class RibbonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // POST: Search/SearchClients
        [HttpPost]
        public string UserIsAdmin([FromBody] string args)
        {
            try
            {
                string username = args;
                bool isAdmin = SUP_Library.DatabaseConnection.isAdmin(username);
                var json = JsonConvert.SerializeObject(isAdmin);
                return json;
            }
            catch (Exception e)
            {
                return "FAAAAAILLL";
            }
        }
    }
}