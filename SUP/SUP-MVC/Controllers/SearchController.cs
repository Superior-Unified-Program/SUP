using System;
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
            ResetTimeout();
            try
            {
                string[] separatedArgs = args.Split(',');
                if (separatedArgs.Length != 4)
                {
                    throw(new Exception("Oopsie"));
                }

				var organizations = separatedArgs[2].Split(';');
				if (Array.IndexOf(organizations, "Education - All") >= 0)
				{
					organizations = organizations.Where(val => val != "Education - All").ToArray();
					string[] modifiedOrgs = new string[organizations.Length + 4];
					int i = 0;
					while(i < organizations.Length)
					{
						modifiedOrgs[i] = organizations[i];
						i++;
					}
					modifiedOrgs[i++] = "Elementary School";
					modifiedOrgs[i++] = "Middle School";
					modifiedOrgs[i++] = "High School";
					modifiedOrgs[i++] = "Higher Education";
					organizations = modifiedOrgs;
				}

				var Clients = DatabaseConnection.QueryClientFull(separatedArgs[1], separatedArgs[0], organizations[0]);
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
                            clientCount -= 1;
                            i -= 1;
                        }
                    }
                }
                if (organizations[0].Length == 0)
                     Clients.Sort((a, b) => string.Compare(a.First_Name, b.First_Name));


                // filter by organization

                List<Client> filteredClients = new List<Client>();
                foreach (string organization in organizations)
                {
                    foreach (var client in Clients)
                    {
                        var stringifiedOrganizations = client.Organizations.Select(o => o.Org_Type);
                        if (stringifiedOrganizations.Contains(organization))
                        {
                            if (!filteredClients.Contains(client))
                            {
                                filteredClients.Add(client);
                            }
                        }
                    }
                }
                filteredClients.Sort((a, b) => string.Compare(a.First_Name, b.First_Name));
                var json = (organizations[0].Length > 0) ? JsonConvert.SerializeObject(filteredClients) : JsonConvert.SerializeObject(Clients);
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
            ResetTimeout();
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
                ExportFile.CreateExcelFile(clientArray, out fileName);

                // Test guest parking template letter if placed in template folder in c:\users\public\documents\templates
                // Using opening and closing tokens < > and search fields defined in spec document such as <firstname> and <lastname>
                //List<string> templateNames = Merge.getTemplateNames();
                //string zipfile;
                //if (templateNames.Count>=1)
                //Merge.merge(clientArray, templateNames[0], out zipfile);
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

        // POST: Search/TemplateMerge
        [HttpPost]
        public string TemplateMerge([FromBody] string args)
        {
            ResetTimeout();
            try
            {
                string[] separatedArgs = args.Split(' ');

                if (separatedArgs.Length <= 0)
                {
                    throw (new Exception("Oopsie"));
                }

				var fileName = separatedArgs[0];

                var clientArray = new List<Client>();
				for(var i = 1; i < separatedArgs.Length; i++)
                {
                    var currentClient = DatabaseConnection.GetClientByIdFull(separatedArgs[i]);
                    if (currentClient != null)
                        clientArray.Add(currentClient);
                }

                // Test guest parking template letter if placed in template folder in c:\users\public\documents\templates
                // Using opening and closing tokens < > and search fields defined in spec document such as <firstname> and <lastname>
                
                //List<string> templateNames = Merge.getTemplateNames();
                string zipFile = "";
                //if (templateNames.Count >= 1) 
				int documentsGenerated = Merge.merge(clientArray, fileName, out zipFile);
                //else return "No person selected !!!!";

                var json = JsonConvert.SerializeObject(zipFile);
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
            if (TempData["LoginDate"] != null && TempData["LoginTime"] != null)
            {
                int minutesTillLogout = 10;

                DateTime loadedDateTime = DateTime.ParseExact(TempData["LoginDate"].ToString(), "d", null);
                DateTime loadedTime = DateTime.ParseExact(TempData["LoginTime"].ToString(), "t", null);
                if (loadedDateTime.ToShortDateString().Equals(DateTime.Now.ToShortDateString()))
                {
                    var currentTime = DateTime.ParseExact(DateTime.Now.ToShortTimeString(), "t", null);
                    var tooLate = loadedTime;
                    tooLate = tooLate.AddMinutes(minutesTillLogout);
                    if (currentTime.TimeOfDay > tooLate.TimeOfDay)
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
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpGet]
        public ActionResult DownloadExcel(string fileName)
        {
            ResetTimeout();
            //Get the temp folder and file path in server
            string savePath = @"C:\Users\Public\Documents\SUPExport";
            string fullPath = Path.Combine(savePath, fileName);
            byte[] fileByteArray = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);    // detele the excel file
            return File(fileByteArray, "application/vnd.ms-excel", fileName);
        }

        [HttpGet]
        public ActionResult DownloadZipFile(string fileName)
        {
            ResetTimeout();
            //Get the temp folder and file path in server
            string saveAsName = fileName.Substring(fileName.LastIndexOf('\\')+1);
            string savePath = @"C:\Users\Public\Documents\SUPExport";
            string fullPath = Path.Combine(savePath, fileName);
            byte[] fileByteArray = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);    // detele the excel file
            return File(fileByteArray, "application/zip", saveAsName);
        }

		[HttpPost]
		public string GetTemplateNames([FromBody] string args)
		{
			try
			{
				var nameList = SUP_Library.Merge.getTemplateNames();
				var json = JsonConvert.SerializeObject(nameList);
				return json;
			}
			catch (Exception e)
			{
				throw e;
				//return "FAAAAAILLL";
			}
		}

		[HttpPost]
		public string GetTemplateInfo([FromBody] string args)
		{
			try
			{
				var nameList = SUP_Library.Merge.getTemplateNames();
				var nameAndDateList = SUP_Library.Merge.getFileModificationDates(nameList);
				var json = JsonConvert.SerializeObject(nameAndDateList);
				return json;
			}
			catch (Exception e)
			{
				throw e;
				//return "FAAAAAILLL";
			}
		}

		[HttpPost]
		public async Task<IActionResult> UploadFile(IFormFile file)
		{
            ResetTimeout();

            bool isDocx = Microsoft.VisualBasic.CompilerServices.LikeOperator.LikeString(file.FileName, "*.docx", Microsoft.VisualBasic.CompareMethod.Binary);
			if (isDocx)
				{
				string templatePath = (Directory.GetCurrentDirectory()).Replace("SUP-MVC", "SUP_Library\\Templates");
                string fileName = Path.GetFileName(file.FileName);
				string filePath = templatePath + "\\" + fileName;
				long size = file.Length;
				//var filePath = file.Name;


				if (size > 0)
				{
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await file.CopyToAsync(stream);
					}
				}


				// process uploaded files
				// Don't rely on or trust the FileName property without validation.
				return RedirectToAction("Search");
			}
			return Ok(new { Error = "File was not a .docx file."});
		}

        private void ResetTimeout()
        {
            TempData["LoginDate"] = DateTime.Now.ToShortDateString();
            TempData["LoginTime"] = DateTime.Now.ToShortTimeString();
        }
	}
}