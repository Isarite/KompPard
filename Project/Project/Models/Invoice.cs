using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Invoice
    {
        [ForeignKey("Cart")]
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public virtual Cart Cart { get; set; }
    }
}
