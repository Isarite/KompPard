using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class MM_InventoryItem_Category
    {
        [ForeignKey("InventoryItem")]
        public int InventoryItemId { get; set; }

        [ForeignKey("InventoryItemCategory")]
        public int CategoryId { get; set; }

        public virtual InventoryItem InventoryItem { get; set; }
        public virtual InventoryItemCategory InventoryItemCategory { get; set; }
    }
}
