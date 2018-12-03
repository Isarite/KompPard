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
        [Range(0, 5, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Rating { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual InventoryItem InventoryItem { get; set; }
    }
}
