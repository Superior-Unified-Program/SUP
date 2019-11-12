using System;

namespace SUP_MVC.Models.AddUser
{
    public class AddUserViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
