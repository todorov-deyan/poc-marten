using PocMarten.Api.Aggregates.Helpdesk.Models;

namespace PocMarten.Api.Aggregates.Helpdesk.Events
{
    public record IncidentLogged(
        Guid IncidentId,
        Guid CustomerId,
        Contact Contact,
        string Description,
        Guid LoggedBy,
        DateTimeOffset LoggedAt
    );
}
