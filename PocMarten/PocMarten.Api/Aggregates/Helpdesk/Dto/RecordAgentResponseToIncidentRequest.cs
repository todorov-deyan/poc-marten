namespace PocMarten.Api.Aggregates.Helpdesk.Dto
{
    public record RecordAgentResponseToIncidentRequest(
        string Content,
        bool VisibleToCustomer
    );
}
