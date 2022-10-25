using PocMarten.Api.Aggregates.Helpdesk.Models;

namespace PocMarten.Api.Aggregates.Helpdesk.Service.Models
{
    public record PrioritiseIncident(
        Guid IncidentId,
        IncidentPriority Priority,
        Guid PrioritisedBy
    );
}
