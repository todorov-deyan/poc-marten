using PocMarten.Api.Aggregates.Helpdesk.Models;

namespace PocMarten.Api.Aggregates.Helpdesk.Events
{
    public record IncidentCategorised(
        Guid IncidentId,
        IncidentCategory Category,
        Guid CategorisedBy,
        DateTimeOffset CategorisedAt
    );
}
