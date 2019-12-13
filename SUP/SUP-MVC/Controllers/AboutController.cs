using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SUP_MVC.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult Index()
        {
            return View();
        }

        // GET: About/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: About/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: About/Create
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

        // GET: About/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: About/Edit/5
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

        // GET: About/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: About/Delete/5
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

		public ActionResult About()
		{
            if (TempData["LoginDate"] != null && TempData["LoginTime"] != null)
            {
                int minutesTillLogout = 10;

                DateTime loadedDateTime = DateTime.ParseExact(TempData["LoginDate"].ToString(), "d", null);
                DateTime loadedTime = DateTime.ParseExact(TempData["LoginTime"].ToString(), "t", null);
                if (loadedDateTime.ToShortDateString().Equals(DateTime.Now.ToShortDateString()))
                {
                    var currentTime = DateTime.Now.Minute;
                    if (Math.Abs(loadedTime.Minute - currentTime) > minutesTillLogout)
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
        private void ResetTimeout()
        {
            TempData["LoginDate"] = DateTime.Now.ToShortDateString();
            TempData["LoginTime"] = DateTime.Now.ToShortTimeString();
        }
    }
}