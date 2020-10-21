using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Chat_Application.Areas.Identity.Data
{
    public class Chat_ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "varchar(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "varchar(100)")]
        public string LastName { get; set; }
    }
}
