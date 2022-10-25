using PocMarten.Api.Aggregates.Helpdesk.Events;
using PocMarten.Api.Aggregates.Helpdesk.Service.Models;

namespace PocMarten.Api.Aggregates.Helpdesk.Service
{
    public static class IncidentService
    {
        public static IncidentLogged Handle(LogIncident command)
        {
            var (incidentId, customerId, contact, description, loggedBy) = command;

            return new IncidentLogged(incidentId, customerId, contact, description, loggedBy, DateTimeOffset.UtcNow);
        }
    }
}
