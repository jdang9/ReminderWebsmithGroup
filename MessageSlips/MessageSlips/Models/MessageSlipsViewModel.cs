using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace MessageSlips.Models
{
    public class MessageSlipsViewModel
    {
        private MessageSlipsWSGEntities db = new MessageSlipsWSGEntities();
        public int MessageId { set; get; }
        public string Sender { set; get; }
        public string Receiver { set; get; }
        public string Categories { set; get; }
        public string Date { set; get; }
        public string Time { set; get; }
        public string Phone { set; get; }
        public string Message { set; get; }
        public string Email { set; get; }
        public string Other { set; get; }
        public string Username { set; get; }

        public IEnumerable<MessageSlip> GetMessage() { 
            return db.MessageSlips.ToList();
        }

        public MessageSlip GetMessage(int currentMID)
        {
            return db.MessageSlips.Where(m => m.mID == currentMID).FirstOrDefault();
        }
    }

 
}