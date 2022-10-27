using Marten;
using Marten.Events;
using Marten.Events.Aggregation;
using Marten.Events.Projections;

using PocMarten.Api.Aggregates.Helpdesk.Events;

namespace PocMarten.Api.Aggregates.Helpdesk.Models.GetCustomerIncidentsSummary
{
    public class CustomerIncidentsSummaryGrouper:IAggregateGrouper<Guid>
    {
        private readonly Type[] eventTypes =
        {
            typeof(IEvent<IncidentResolved>), typeof(IEvent<ResolutionAcknowledgedByCustomer>),
            typeof(IEvent<IncidentClosed>)
        };

        public async Task Group(IQuerySession session, IEnumerable<IEvent> events, ITenantSliceGroup<Guid> grouping)
        {
            var filteredEvents = events
                .Where(ev => eventTypes.Contains(ev.GetType()))
                .ToList();

            if (!filteredEvents.Any())
                return;

            var incidentIds = filteredEvents
                .Select(e => e.StreamId)
                .ToList();

            var result = await session.Events
                .QueryRawEventDataOnly<IncidentLogged>()
                .Where(e => incidentIds.Contains(e.IncidentId))
                .Select(x => new { x.IncidentId, x.CustomerId })
                .ToListAsync();

            foreach (var group in result.Select(g => 
                         new
                         {
                             g.CustomerId, 
                             Events = filteredEvents.Where(ev => ev.StreamId == g.IncidentId)
                         }))
            {
                grouping.AddEvents(group.CustomerId, group.Events);
            }
        }
    }
}
