namespace PocMarten.Api.Aggregates.Helpdesk.Models.GetIncidentHistory
{
    public record IncidentHistory(
        Guid Id,
        Guid IncidentId,
        string Description
    );
}
