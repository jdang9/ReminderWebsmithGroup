using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageSlips.Models
{
    public class NewMessage
    {
        public string GetSender { get; set; }
        public string GetReceiver { get; set; }
        public string GetCategories { get; set; }
        public string GetDate { get; set; }
        public string GetTime { get; set; }
        public string GetPhoneNum { get; set; }
        public string GetMessage { get; set; }
        public string GetEmail { get; set; }
        public string GetOther { get; set; }
        public string GetUsername { get; set; }
    }
}