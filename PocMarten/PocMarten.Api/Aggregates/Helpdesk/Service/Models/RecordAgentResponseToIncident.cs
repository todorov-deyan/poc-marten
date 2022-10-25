using PocMarten.Api.Aggregates.Helpdesk.Models;

namespace PocMarten.Api.Aggregates.Helpdesk.Service.Models
{
    public record RecordAgentResponseToIncident(
        Guid IncidentId,
        IncidentResponse.FromAgent Response
    );
}
