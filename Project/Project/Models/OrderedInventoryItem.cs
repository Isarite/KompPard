using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class OrderedInventoryItem
    {
        [ForeignKey("InventoryItem")]
        public int ItemId { get; set; }
        
        [ForeignKey("Cart")]
        public Guid CartId { get; set; }

        public int Quantity { get; set; }
        public string Comment { get; set; }

        public virtual InventoryItem InventoryItem { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
