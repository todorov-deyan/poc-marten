using PocMarten.Api.Aggregates.Helpdesk.Models;

namespace PocMarten.Api.Aggregates.Helpdesk.Service.Models
{
    public record LogIncident(
        Guid IncidentId,
        Guid CustomerId,
        Contact Contact,
        string Description,
        Guid LoggedBy
    );
}
