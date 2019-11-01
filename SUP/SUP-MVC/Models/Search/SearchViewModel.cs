using System;

namespace SUP_MVC.Models.Search
{
	public class SearchViewModel
	{
        
		public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Organization { get; set; }

        public string[] storedClientIds { get; set; }

		public string[] firstNames { get; set; }
        public string[] lastNames { get; set; }

        //public bool selected
    }
}
