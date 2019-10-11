using System;

namespace SUP_MVC.Models.About
{
	public class AboutViewModel
	{
		public string RequestId { get; set; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
	}
}
