namespace PocMarten.Api.Aggregates.Helpdesk.Models.GetCustomerIncidentsSummary
{
    public class CustomerIncidentsSummary
    {
        public Guid Id { get; set; }
        
        public int Pending { get; set; }
        
        public int Resolved { get; set; }
        
        public int Acknowledged { get; set; }
     
        public int Closed { get; set; }
    }
}
