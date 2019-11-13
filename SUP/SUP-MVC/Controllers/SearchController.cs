﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SUP_Library;
using SUP_MVC.Models.Search;
using System.Net;
using SUP_Library.DBComponent;
using System.IO;

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

                var clientArray = new List<Client>();
                foreach(var ID in separatedArgs)
                {
                    var currentClient = DatabaseConnection.GetClientByIdFull(ID);
                    if (currentClient != null)
                        clientArray.Add(currentClient);
                }

                string fileName;
                ExportFile.CreateExcelFile2(clientArray, out fileName);

                // Test guest parking template letter if placed in template folder in c:\users\public\documents\templates
                // Using opening and closing tokens < > and search fields defined in spec document such as <firstname> and <lastname>
                //string zipfile;
                //Merge.merge(clientArray, "Guest_Parking_Letter_Template.docx", out zipfile);
                

                /*
                using (WebClient client = new WebClient())
                {
                    string eFileName = "ExcelFile" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.ToString("h_mm_ss_tt") + ".xlsm";
                    client.DownloadFile("http://sup.simple-url.com/",
                                        @"C:\Users\Public\Documents\" + eFileName + ".xlsx");
                }
                */

                //TODO: return value should describe whether or not the process worked to the client.
                //  The below line is meaningless until then.
                var json = JsonConvert.SerializeObject(fileName);
                return json;

          
            }
            catch (Exception e)
            {
                throw e;
                //return "FAAAAAILLL";
            }
        }

        public ActionResult Search()
		{

			return View();
		}

        [HttpGet]
        public ActionResult Download(string fileName)
        {
            //Get the temp folder and file path in server
            string savePath = @"C:\Users\Public\Documents\SUPExport";
            string fullPath = Path.Combine(savePath, fileName);
            byte[] fileByteArray = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);    // detele the excel file
            return File(fileByteArray, "application/vnd.ms-excel", fileName);
        }
    }
}