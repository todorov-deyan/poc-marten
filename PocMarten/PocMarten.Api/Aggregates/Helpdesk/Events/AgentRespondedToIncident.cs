using PocMarten.Api.Aggregates.Helpdesk.Models;

namespace PocMarten.Api.Aggregates.Helpdesk.Events
{
    public record AgentRespondedToIncident(
        Guid IncidentId,
        IncidentResponse.FromAgent Response,
        DateTimeOffset RespondedAt
    );
}
