using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Report
    {
        public Guid Id { get; set; }

        public ReportType Type { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }

        public string ContentsHtml { get; set; }

        [ForeignKey("Creator")]
        public string CreatorId { get; set; }
        public virtual ApplicationUser Creator { get; set; }
    }

    public enum ReportType
    {
        Clients = 0, Profits = 1, MostPopularServices = 2, MostPopularGoods = 3, AllOrders = 4, ActiveOrders = 5
    }
}
