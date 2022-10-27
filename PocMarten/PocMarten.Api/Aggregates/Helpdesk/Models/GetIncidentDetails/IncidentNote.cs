namespace PocMarten.Api.Aggregates.Helpdesk.Models.GetIncidentDetails
{
    public record IncidentNote(
        IncidentNoteType Type,
        Guid From,
        string Content,
        bool VisibleToCustomer
    );
}
