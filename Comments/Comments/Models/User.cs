using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Comments.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string HomePage { get; set; }
        public string Captcha { get; set; }
        public string IpAddres { get; set; }
    }
}