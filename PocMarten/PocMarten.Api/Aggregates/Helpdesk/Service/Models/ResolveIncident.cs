using PocMarten.Api.Aggregates.Helpdesk.Models;

namespace PocMarten.Api.Aggregates.Helpdesk.Service.Models
{
    public record ResolveIncident(
        Guid IncidentId,
        ResolutionType Resolution,
        Guid ResolvedBy
    );
}
