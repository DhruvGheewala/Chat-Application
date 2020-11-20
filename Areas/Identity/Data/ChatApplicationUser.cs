using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ChatApplication.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatApplication.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ChatApplicationUser class
    public class ChatApplicationUser : IdentityUser
    {
        public ChatApplicationUser()
        {
            Messages = new HashSet<Message>();
        }

        [PersonalData]
        [Column(TypeName = "varchar(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "varchar(100)")]
        public string LastName { get; set; }

        //  1 - Many AppUser   ||  Message
        public virtual ICollection<Message> Messages { get; set; }
    }
}
