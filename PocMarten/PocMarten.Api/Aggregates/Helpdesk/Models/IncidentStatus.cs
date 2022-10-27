namespace PocMarten.Api.Aggregates.Helpdesk.Models
{
    public enum IncidentStatus
    {
        None = 0,
        Pending = 1,
        Resolved = 8,
        ResolutionAcknowledgedByCustomer = 16,
        Closed = 32
    }
}
