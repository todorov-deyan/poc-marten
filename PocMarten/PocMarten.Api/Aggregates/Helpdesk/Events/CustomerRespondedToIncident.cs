using PocMarten.Api.Aggregates.Helpdesk.Models;

namespace PocMarten.Api.Aggregates.Helpdesk.Events
{
    public record CustomerRespondedToIncident(
        Guid IncidentId,
        IncidentResponse.FromCustomer Response,
        DateTimeOffset RespondedAt
    );
}
