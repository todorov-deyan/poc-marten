using Marten.Events.Projections;
using PocMarten.Api.Aggregates.Helpdesk.Events;

namespace PocMarten.Api.Aggregates.Helpdesk.Models.GetCustomerIncidentsSummary
{
    public class CustomerIncidentsSummaryProjection : MultiStreamAggregation<CustomerIncidentsSummary, Guid>
    {
        public CustomerIncidentsSummaryProjection()
        {
            Identity<IncidentLogged>(e => e.CustomerId);
            CustomGrouping(new CustomerIncidentsSummaryGrouper());
        }

        public void Apply(IncidentLogged logged, CustomerIncidentsSummary current)
        {
            current.Pending++;
        }

        public void Apply(IncidentResolved resolved, CustomerIncidentsSummary current)
        {
            current.Pending--;
            current.Resolved++;
        }

        public void Apply(ResolutionAcknowledgedByCustomer acknowledged, CustomerIncidentsSummary current)
        {
            current.Resolved--;
            current.Acknowledged++;
        }

        public void Apply(IncidentClosed closed, CustomerIncidentsSummary current)
        {
            current.Acknowledged--;
            current.Closed++;
        }
    }
}
