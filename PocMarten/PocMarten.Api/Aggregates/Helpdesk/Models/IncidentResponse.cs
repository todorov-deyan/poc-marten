namespace PocMarten.Api.Aggregates.Helpdesk.Models
{
    public abstract record IncidentResponse
    {
        public record FromAgent(
            Guid AgentId,
            string Content,
            bool VisibleToCustomer
        ): IncidentResponse(Content);

        public record FromCustomer(
            Guid CustomerId,
            string Content
        ): IncidentResponse(Content);

        public string Content { get; init; }

        private IncidentResponse(string content)
        {
            Content = content;
        }
    }
}
