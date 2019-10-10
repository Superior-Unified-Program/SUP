using System;

namespace SUP_MVC.Models.AddClient
{
	public class AddClientViewModel
	{
		public string RequestId { get; set; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
	}
}
