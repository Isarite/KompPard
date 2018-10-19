using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class OrderedServiceItem
    {
        [ForeignKey("ServiceItem")]
        public int ServiceId { get; set; }

        [ForeignKey("Cart")]
        public Guid CartId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ServiceItem ServiceItem { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
