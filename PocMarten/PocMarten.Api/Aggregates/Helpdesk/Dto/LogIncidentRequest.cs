using PocMarten.Api.Aggregates.Helpdesk.Models;

namespace PocMarten.Api.Aggregates.Helpdesk.Dto
{
    public record LogIncidentRequest(
        Contact Contact,
        string Description
    );
}
