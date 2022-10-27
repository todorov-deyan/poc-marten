namespace PocMarten.Api.Aggregates.Helpdesk.Models.GetIncidentDetails
{
    public record IncidentDetails(
        Guid Id,
        Guid CustomerId,
        IncidentStatus Status,
        IncidentNote[] Notes,
        IncidentCategory? Category = null,
        IncidentPriority? Priority = null,
        Guid? AgentId = null,
        int Version = 1
    );
}
