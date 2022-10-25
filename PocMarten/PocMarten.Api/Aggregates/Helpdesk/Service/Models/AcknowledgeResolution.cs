namespace PocMarten.Api.Aggregates.Helpdesk.Service.Models
{
    public record AcknowledgeResolution(
        Guid IncidentId,
        Guid AcknowledgedBy
    );
}
