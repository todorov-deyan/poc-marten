using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Order.Models
{
    public class Order : Aggregate
    {
        public Guid Id { get; set; }

        public Priority Priority { get; set; }

        public string CustomerId { get; set; }

        public IList<OrderDetail> Details { get; set; } = new List<OrderDetail>();
    }
}