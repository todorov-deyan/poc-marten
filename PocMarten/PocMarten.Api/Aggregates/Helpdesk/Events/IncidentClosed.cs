namespace PocMarten.Api.Aggregates.Helpdesk.Events
{
    public record IncidentClosed(
        Guid IncidentId,
        Guid ClosedBy,
        DateTimeOffset ClosedAt
    );
}
