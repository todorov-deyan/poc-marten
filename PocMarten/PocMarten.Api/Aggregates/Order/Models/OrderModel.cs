using PocMarten.Api.Aggregates.Order.Events;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Order.Models
{
    public class OrderModel : Aggregate
    {
        public Priority Priority { get; set; }

        public string CustomerId { get; set; }

        public IList<OrderDetail> Details { get; set; } = new List<OrderDetail>();

        public void Apply(OrderPriorityHigh @event)
        {
            Priority = @event.Priority;
        }

        public void Apply(OrderPriorityLow @event)
        {
            Priority = @event.Priority;
        }
    }
}