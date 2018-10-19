using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImgPath { get; set; }

        // Quantity remainig in the warehouse
        public int Stock { get; set; }

        // Rating of the item 1-100. Updates on create of new review
        public int Rating { get; set; }

        [ForeignKey("Discount")]
        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }
    }
}