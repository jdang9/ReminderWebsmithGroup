using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MessageSlips.Models
{
    public class Index
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }

}