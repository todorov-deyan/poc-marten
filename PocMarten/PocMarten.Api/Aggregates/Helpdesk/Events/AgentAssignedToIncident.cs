namespace PocMarten.Api.Aggregates.Helpdesk.Events
{
    public record AgentAssignedToIncident(
        Guid IncidentId,
        Guid AgentId,
        DateTimeOffset AssignedAt
    );
}
