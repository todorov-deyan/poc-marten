using PocMarten.Api.Aggregates.Helpdesk.Events;

namespace PocMarten.Api.Aggregates.Helpdesk.Models
{
    public record Incident(
        Guid Id,
        IncidentStatus Status = IncidentStatus.Pending,
        bool HasOutstandingResponseToCustomer = false,
        IncidentCategory? Category = null,
        IncidentPriority? Priority = null,
        Guid? AgentId = null,
        int Version = 1
    )
    {
        public static Incident Create(IncidentLogged logged) => new(logged.IncidentId);

        public Incident Apply(IncidentCategorised categorised) => this with { Category = categorised.Category };

        public Incident Apply(IncidentPrioritised prioritised) => this with { Priority = prioritised.Priority };

        public Incident Apply(AgentRespondedToIncident agentResponded) => this with { HasOutstandingResponseToCustomer = false };

        public Incident Apply(CustomerRespondedToIncident customerResponded) => this with { HasOutstandingResponseToCustomer = true };

        public Incident Apply(IncidentResolved resolved) => this with { Status = IncidentStatus.Resolved };

        public Incident Apply(ResolutionAcknowledgedByCustomer acknowledged) => this with { Status = IncidentStatus.ResolutionAcknowledgedByCustomer };

        public Incident Apply(IncidentClosed closed) => this with { Status = IncidentStatus.Closed };
    }
}
