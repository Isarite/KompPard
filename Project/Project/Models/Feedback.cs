using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Feedback
    {
        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("InventoryItem")]
        public int ItemId { get; set; }

        public string Comment { get; set; }
        public int Rating { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual InventoryItem InventoryItem { get; set; }
    }
}
