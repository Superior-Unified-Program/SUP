using System;

namespace SUP_MVC.Models.Login
{
	public class LoginViewModel
	{
		public string RequestId { get; set; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);


	}
}
