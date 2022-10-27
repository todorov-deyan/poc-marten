using PocMarten.Api.Aggregates.Helpdesk.Events;
using PocMarten.Api.Aggregates.Helpdesk.Models;
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

        public static IncidentCategorised Handle(Incident current, CategoriseIncident command)
        {
            IsIncidentStatusClosed(current);

            var (incidentId, incidentCategory, categorisedBy) = command;

            return new IncidentCategorised(incidentId, incidentCategory, categorisedBy, DateTimeOffset.UtcNow);
        }

        public static IncidentPrioritised Handle(Incident current, PrioritiseIncident command)
        {
            IsIncidentStatusClosed(current);

            var (incidentId, incidentPriority, prioritisedBy) = command;

            return new IncidentPrioritised(incidentId, incidentPriority, prioritisedBy, DateTimeOffset.UtcNow);
        }

        public static AgentAssignedToIncident Handle(Incident current, AssignAgentToIncident command)
        {
            IsIncidentStatusClosed(current);

            var (incidentId, agentId) = command;

            return new AgentAssignedToIncident(incidentId, agentId, DateTimeOffset.UtcNow);
        }

        public static CustomerRespondedToIncident Handle(Incident current, RecordCustomerResponseToIncident command)
        {
            IsIncidentStatusClosed(current);

            var (incidentId, response) = command;

            return new CustomerRespondedToIncident(incidentId, response, DateTimeOffset.UtcNow);
        }

        public static AgentRespondedToIncident Handle(Incident current, RecordAgentResponseToIncident command)
        {
            IsIncidentStatusClosed(current);

            var (incidentId, response) = command;

            return new AgentRespondedToIncident(incidentId, response, DateTimeOffset.UtcNow);
        }

        public static IncidentResolved Handle(Incident current, ResolveIncident command)
        {
            if (current.Status is IncidentStatus.Resolved or IncidentStatus.Closed)
                throw new InvalidOperationException("Cannot resolve already resolved or closed incident");

            if (current.HasOutstandingResponseToCustomer)
                throw new InvalidOperationException("Cannot resolve incident that has outstanding responses to customer");

            var (incidentId, resolution, resolvedBy) = command;

            return new IncidentResolved(incidentId, resolution, resolvedBy, DateTimeOffset.UtcNow);
        }

        public static ResolutionAcknowledgedByCustomer Handle(Incident current, AcknowledgeResolution command)
        {
            if (current.Status is not IncidentStatus.Resolved)
                throw new InvalidOperationException("Only resolved incident can be acknowledged");

            var (incidentId, acknowledgedBy) = command;

            return new ResolutionAcknowledgedByCustomer(incidentId, acknowledgedBy, DateTimeOffset.UtcNow);
        }

        public static IncidentClosed Handle(Incident current, CloseIncident command)
        {
            if (current.Status is not IncidentStatus.ResolutionAcknowledgedByCustomer)
                throw new InvalidOperationException("Only incident with acknowledged resolution can be closed");

            if (current.HasOutstandingResponseToCustomer)
                throw new InvalidOperationException("Cannot close incident that has outstanding responses to customer");

            var (incidentId, acknowledgedBy) = command;

            return new IncidentClosed(incidentId, acknowledgedBy, DateTimeOffset.UtcNow);
        }

        private static void IsIncidentStatusClosed(Incident current)
        {
            if (current.Status == IncidentStatus.Closed)
                throw new InvalidOperationException("Incident is already closed");
        }
    }
}
