using System;

namespace SUP_MVC.Models.Ribbon
{
	public class RibbonViewModel
	{
		public string RequestId { get; set; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
	}
}
