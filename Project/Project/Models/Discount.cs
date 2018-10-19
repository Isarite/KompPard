namespace Project.Models
{
    public class Discount
    {
        public int Id { get; set; }

        // ##.##%
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
