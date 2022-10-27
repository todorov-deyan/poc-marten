using PocMarten.Api.Aggregates.Helpdesk.Models;

namespace PocMarten.Api.Aggregates.Helpdesk.Dto
{
    public record CategoriseIncidentRequest(
        IncidentCategory Category
    );
}
