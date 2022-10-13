using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Order.Models
{
    public class OrderDetail : Aggregate
    {
        public string PartNumber { get; set; }

        public int Number { get; set; }
    }
}
