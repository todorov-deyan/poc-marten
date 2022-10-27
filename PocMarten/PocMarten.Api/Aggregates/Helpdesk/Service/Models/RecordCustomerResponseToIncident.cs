using PocMarten.Api.Aggregates.Helpdesk.Models;

namespace PocMarten.Api.Aggregates.Helpdesk.Service.Models
{
    public record RecordCustomerResponseToIncident(
        Guid IncidentId,
        IncidentResponse.FromCustomer Response
    );
}
