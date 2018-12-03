using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class MM_InventoryItem_Category
    {
        [ForeignKey("InventoryItem")]
        public int InventoryItemId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual InventoryItemCategory InventoryItemCategory { get; set; }
    }
}
