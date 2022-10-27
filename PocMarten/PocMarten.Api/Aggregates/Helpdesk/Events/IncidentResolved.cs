using PocMarten.Api.Aggregates.Helpdesk.Models;

namespace PocMarten.Api.Aggregates.Helpdesk.Events
{
    public record IncidentResolved(
        Guid IncidentId,
        ResolutionType Resolution,
        Guid ResolvedBy,
        DateTimeOffset ResolvedAt
    );
}
