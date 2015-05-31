using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageSlips.Models
{
    public class ClientViewModel
    {
        public int clientID { get; set; }

        public string clientName { get; set; }

        public IEnumerable<Client> Clients { get; set; }
    }
}
