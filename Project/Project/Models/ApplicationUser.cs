using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Id, Email <- base

        public DateTime RegisterDate { get; set; }
        public DateTime LastLoginDate { get; set; }

        public decimal Balance { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Address { get; set; }

        // Subscribtion to the email
        public bool IsSubscribed { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}
