using PocMarten.Api.Aggregates.Helpdesk.Models;

namespace PocMarten.Api.Aggregates.Helpdesk.Service.Models
{
    public record CategoriseIncident(
        Guid IncidentId,
        IncidentCategory Category,
        Guid CategorisedBy
    );
}
