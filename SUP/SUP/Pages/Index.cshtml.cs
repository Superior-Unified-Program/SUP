using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SUP.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        /*
        public string Result1 { set; get; }
        public int    NumResults { set; get; }
        */
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            /*
             * Example round trip
            var fn = Request.Query["firstName"];
            var ln = Request.Query["lastName"];

            List<SUP_Library.DBComponent.Client> clients = SUP_Library.DatabaseConnection.queryClient(ln, fn,null);

            NumResults = clients.Count;

            if (clients != null && clients.Count != 0)
                Result1 = clients[0].First_Name + " " + clients[0].Middle_initial + " " + clients[0].Last_Name + " and works for " + clients[0].Org.Org_Name;
            else
                Result1 = "No results";
            */
        }
    }
}
