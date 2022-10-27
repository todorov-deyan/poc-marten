using PocMarten.Api.Aggregates.Helpdesk.Models;

namespace PocMarten.Api.Aggregates.Helpdesk.Dto
{
    public record PrioritiseIncidentRequest(
        IncidentPriority Priority
    );
}
