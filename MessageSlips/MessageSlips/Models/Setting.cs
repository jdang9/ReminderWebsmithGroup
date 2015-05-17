using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageSlips.Models
{
    public class Setting
    {
        public string GetUser { get; set; }
        public string GetPassword { get; set; }
        public string GetEmail { get; set; }
        public bool GetAdmin { get; set; }
        public string GetFirstName { get; set; }
        public string GetLastName { get; set; }

    }

}