using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Invoice
    {
        [ForeignKey("Cart")]
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? PaymentDate { get; set; }

        [Required(ErrorMessage = "Please enter an address for delivery")]
        public string DeliveryAddress { get; set; }

        [Required(ErrorMessage = "Please provide an email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please input a phone number")]
        public string PhoneNumber { get; set; }

        public virtual Cart Cart { get; set; }
    }
}
