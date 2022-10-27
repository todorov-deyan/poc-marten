namespace PocMarten.Api.Aggregates.Helpdesk.Models.GetIncidentShortInfo
{
    public record IncidentShortInfo(
        Guid Id,
        Guid CustomerId,
        IncidentStatus Status,
        int NotesCount,
        IncidentCategory? Category = null,
        IncidentPriority? Priority = null
    );
}
