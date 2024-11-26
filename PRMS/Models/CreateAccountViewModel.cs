using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PRMS.Models
{
    public class CreateAccountViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
    }

}