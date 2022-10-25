using PocMarten.Api.Aggregates.Helpdesk.Models;

namespace PocMarten.Api.Aggregates.Helpdesk.Events
{
    public record IncidentPrioritised(
        Guid IncidentId,
        IncidentPriority Priority,
        Guid PrioritisedBy,
        DateTimeOffset PrioritisedAt
    );
}
