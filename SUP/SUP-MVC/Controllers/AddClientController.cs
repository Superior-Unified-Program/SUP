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

		[HttpPost]
		public string CheckForNearMatch([FromBody] string args)
		{
			string[] separatedArgs = args.Split(',');
			if (separatedArgs.Length < 2)
			{
				throw (new Exception("Illegal number of arguments"));
			}
			var firstName = separatedArgs[0];
			var lastName = separatedArgs[1];

			var listOfPossibleMatches = SUP_Library.DatabaseConnection.checkForNearMatch(lastName, firstName);

			var json = JsonConvert.SerializeObject(listOfPossibleMatches);

			return json;
		}
        
        
        // POST: AddClient/UpdateClient
        [HttpPost]
        public int UpdateClient([FromBody] string args)
        {
            ResetTimeout();
            try
            {

                string[] separatedArgs = args.Split(',');
                if (separatedArgs.Length < 13)
                {
                    throw (new Exception("Illegal number of arguments"));
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
                var PersonalEmail = separatedArgs[12];
                var Note = separatedArgs[13];
                var PersonalPhone = separatedArgs[14];
                var Active = separatedArgs[15];
				var Permit = separatedArgs[16];
				var Breakfast = separatedArgs[17];
				var HolidayCard = separatedArgs[18];
				var BusinessEmail = separatedArgs[19];
				var BusinessPhone = separatedArgs[20];
				var AssistantEmail = separatedArgs[21];
				var AssistantPhone = separatedArgs[22];
				var AssistantFirstName = separatedArgs[23];
				var AssistantLastName = separatedArgs[24];
				var Prefix = separatedArgs[25];
				var additionalOrganizationString = separatedArgs[26];
				var splitOrganizations = additionalOrganizationString.Split("|");
				List<SUP_Library.DBComponent.Organization> additionalOrganizations = new List<SUP_Library.DBComponent.Organization>();
				foreach (string org in splitOrganizations)
				{
					if (org != "")
					{
						var orgParts = org.Split(";");
						SUP_Library.DBComponent.Organization orgObj = new SUP_Library.DBComponent.Organization();
						orgObj.Org_Type = orgParts[0];
						orgObj.Org_Name = orgParts[1];
						orgObj.Title = orgParts[2];
						orgObj.Primary = false;
						additionalOrganizations.Add(orgObj);
					}
				}
				

				var Client = DatabaseConnection.GetClientByIdFull(clientId);
                if (Client != null)
                {
                    int intClientId = Int32.Parse(clientId);
                    Client.First_Name = firstName;
                    Client.Last_Name = lastName;
                    if (MiddleInitial?.Length >= 1)
                    {
                        Client.Middle_initial = MiddleInitial.Substring(0,1);
                    }
					SUP_Library.DBComponent.Organization primaryOrg = new SUP_Library.DBComponent.Organization();
					primaryOrg.Org_Type = organization;
					primaryOrg.Org_Name = companyName;
					primaryOrg.Title = title;
					primaryOrg.Primary = true;
					additionalOrganizations.Add(primaryOrg);
					Client.Org = primaryOrg;
                    Client.Email = new SUP_Library.DBComponent.EmailAddress
                    {
                       // Client_ID = intClientId,
                        Business_Email = BusinessEmail,
						Personal_Email = PersonalEmail,
						Assistant_Email = AssistantEmail

                    };
                    Client.Phone = new SUP_Library.DBComponent.PhoneNumber
                    {
                       // Client_ID = intClientId,
                        Business_Phone = BusinessPhone,
						Personal_Phone = PersonalPhone,
						Assistant_Phone = AssistantPhone
                    };
					Client.Organizations = additionalOrganizations;
                   
                    Client.Address.Line1 = Line1;
                    Client.Address.Line2 = Line2;
                    Client.Address.City = City;
                    Client.Address.State = State;
                    Client.Address.Zip = Zip;
                    
					Client.Assistant_First_Name = AssistantFirstName;
					Client.Assistant_Last_Name = AssistantLastName;
					Client.Notes = Note;
					Client.Active = (Active == "true");
					Client.Holiday_Card = (HolidayCard == "true");
					Client.Community_Breakfast = (Breakfast == "true");
					Client.Permit_Num = Permit;
					Client.Prefix = Prefix;
					return DatabaseConnection.updateClient(Client);
                }
                else
                {
                    Client = new SUP_Library.DBComponent.Client();

                    Client.First_Name = firstName;
                    Client.Last_Name = lastName;
					if (MiddleInitial?.Length >= 1)
					{
						Client.Middle_initial = MiddleInitial.Substring(0, 1);
					}
					SUP_Library.DBComponent.Organization primaryOrg = new SUP_Library.DBComponent.Organization();
					primaryOrg.Org_Type = organization;
					primaryOrg.Org_Name = companyName;
					primaryOrg.Title = title;
					primaryOrg.Primary = true;
					additionalOrganizations.Add(primaryOrg);
					Client.Org = primaryOrg;
					Client.Organizations = additionalOrganizations;
					Client.Address = new SUP_Library.DBComponent.Address(Line1, Line2, City, State, Zip);
                    Client.Email = new SUP_Library.DBComponent.EmailAddress
                    {
						Business_Email = BusinessEmail,
						Personal_Email = PersonalEmail,
						Assistant_Email = AssistantEmail
					};
                    Client.Phone = new SUP_Library.DBComponent.PhoneNumber
                    {
						Business_Phone = BusinessPhone,
						Personal_Phone = PersonalPhone,
						Assistant_Phone = AssistantPhone
					};
					Client.Assistant_First_Name = AssistantFirstName;
					Client.Assistant_Last_Name = AssistantLastName;
					Client.Notes = Note;
					Client.Active = (Active == "true");
					Client.Holiday_Card = (HolidayCard == "true");
					Client.Community_Breakfast = (Breakfast == "true");
					Client.Prefix = Prefix;
					return DatabaseConnection.addClient(Client);
                }
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        
        public ActionResult AddClient()
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
                return RedirectToAction("Login","Login");
            }
        }
        private void ResetTimeout()
        {
            TempData["LoginDate"] = DateTime.Now.ToShortDateString();
            TempData["LoginTime"] = DateTime.Now.ToShortTimeString();
        }
    }
}