namespace PocMarten.Api.Aggregates.Helpdesk.Events
{
    public record ResolutionAcknowledgedByCustomer(
        Guid IncidentId,
        Guid AcknowledgedBy,
        DateTimeOffset AcknowledgedAt
    );
}
