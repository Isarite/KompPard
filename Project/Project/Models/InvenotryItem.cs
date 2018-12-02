using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImgPath { get; set; }

        // Quantity remainig in the warehouse
        [DefaultValue("true")]
        public int Stock { get; set; }

        // Rating of the item 1-100. Updates on create of new review
        [DefaultValue("true")]
        public double Rating { get; set; }

        [ForeignKey("Discount")]
        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public InventoryItemCategory Category { get; set; }
    }
}