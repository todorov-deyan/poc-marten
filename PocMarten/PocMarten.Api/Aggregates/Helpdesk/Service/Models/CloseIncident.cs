namespace PocMarten.Api.Aggregates.Helpdesk.Service.Models
{
    public record CloseIncident(
        Guid IncidentId,
        Guid ClosedBy
    );
}
