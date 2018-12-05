namespace Project.Models
{
    public class ServiceItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Time in hours to complete. 0/null = N/A
        public int HourDuration { get; set; }
        public decimal HourPrice { get; set; }
    }
}
