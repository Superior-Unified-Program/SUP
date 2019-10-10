using System;

namespace SUP_MVC.Models.Search
{
	public class SearchViewModel
	{
		public string RequestId { get; set; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
	}
}
