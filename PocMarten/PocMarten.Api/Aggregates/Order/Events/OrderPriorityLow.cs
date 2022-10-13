using PocMarten.Api.Aggregates.Order.Models;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Order.Events
{
    public class OrderPriorityLow : IEventState
    {
        public OrderPriorityLow(Priority priority)
        {
            Priority = priority;
        }

        public Priority Priority { get; init; }
    }
}
