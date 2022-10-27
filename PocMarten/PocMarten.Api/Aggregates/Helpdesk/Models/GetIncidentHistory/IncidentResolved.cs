namespace PocMarten.Api.Aggregates.Helpdesk.Models.GetIncidentHistory
{
    public record IncidentResolved(
        Guid IncidentId,
        ResolutionType Resolution,
        Guid ResolvedBy,
        DateTimeOffset ResolvedAt
    );
}
