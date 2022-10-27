namespace PocMarten.Api.Aggregates.Helpdesk.Service.Models
{
    public record AssignAgentToIncident(
        Guid IncidentId,
        Guid AgentId
    );
}
