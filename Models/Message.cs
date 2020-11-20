using ChatApplication.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApplication.Models
{
    public class Message
    {
        public int id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Receiver { get; set; }

        [Required]
        public string Text { get; set; }
        
        public DateTime When { get; set; }

        public string UserID { get; set; }
        
        public virtual ChatApplicationUser Sender { get; set; }

        public Message()
        {
            When = DateTime.Now;
        }

        public Message(string uname,  string _txt, string _reid)
        {
            this.UserName = uname;
            this.Text = _txt;
            this.Receiver = _reid;
            When = DateTime.Now;
        }
    }
}
