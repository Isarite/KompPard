using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public DateTime LastEditDate { get; set; }
        public bool IsFinal { get; set; }
        public decimal TotalValue { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}