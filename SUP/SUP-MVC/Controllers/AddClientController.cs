﻿using System;
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

        // POST: AddClient/SearchClients
        [HttpPost]
        public string GetClient([FromBody] string args)
        {
            try
            {
                var clientId = args;
                
                var Clients = DatabaseConnection.GetClientByIdFull(clientId);
                var json = JsonConvert.SerializeObject(Clients);

                return json;
            }
            catch (Exception e)
            {
                return "FAAAAAILLL";
            }
        }


        // POST: AddClient/UpdateClient
        [HttpPost]
        public bool UpdateClient([FromBody] string args)
        {
            try
            {

                string[] separatedArgs = args.Split(',');
                if (separatedArgs.Length < 13)
                {
                    throw (new Exception("Oopsie"));
                }
                var clientId = separatedArgs[0];
                var firstName = separatedArgs[1];
                var lastName = separatedArgs[2];
                var MiddleInitial = separatedArgs[3];
                var organization = separatedArgs[4];
                var companyName = separatedArgs[5];
                var title = separatedArgs[6];
                var Line1 = separatedArgs[7];
                var Line2 = separatedArgs[8];
                var City = separatedArgs[9];
                var State = separatedArgs[10];
                var Zip = separatedArgs[11];
                var Email = separatedArgs[12];
                var Note = separatedArgs[13];
                var Phone = separatedArgs[14];
                var Active = separatedArgs[15];
                var Client = DatabaseConnection.GetClientById(clientId);
                if (Client != null)
                {

                    int intClientId = Int32.Parse(clientId);
                    Client.First_Name = firstName;
                    Client.Last_Name = lastName;
                    if (MiddleInitial?.Length >= 1)
                    {
                        Client.Middle_initial = MiddleInitial[0];
                    }
                    Client.Org = new SUP_Library.DBComponent.Organization
                    {
                        Client_ID = intClientId,
                        Org_Name = companyName,
                        Org_Type = organization,
                        Title = title
                    };
                    Client.Email = new SUP_Library.DBComponent.EmailAddress
                    {
                        Client_ID = intClientId,
                        Email = Email
                    };
                    Client.Phone = new SUP_Library.DBComponent.PhoneNumber
                    {
                        Client_ID = intClientId,
                        Number = Phone
                    };
                    Client.Notes = Note;
                    Client.Active = (Active == "true");
                    DatabaseConnection.updateClient(Client);
                }
                else
                {
                    Client = new SUP_Library.DBComponent.Client();

                    Client.First_Name = firstName;
                    Client.Last_Name = lastName;
                    Client.Org = new SUP_Library.DBComponent.Organization
                    {
                        Org_Name = companyName,
                        Org_Type = organization,
                        Title = title
                    };
                    Client.Address = new SUP_Library.DBComponent.Address(Line1, Line2, City, State, Zip);
                    Client.Email = new SUP_Library.DBComponent.EmailAddress
                    {
                        Email = Email
                    };
                    Client.Phone = new SUP_Library.DBComponent.PhoneNumber
                    {
                        Number = Phone
                    };
                    Client.Notes = Note;
                    DatabaseConnection.addClient(Client);
                }


                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public ActionResult AddClient()
		{
			return View();
		}
	}
}