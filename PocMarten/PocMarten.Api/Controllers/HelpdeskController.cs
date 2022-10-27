using Marten;
using Marten.AspNetCore;
using Marten.Schema.Identity;

using Microsoft.AspNetCore.Mvc;

using PocMarten.Api.Aggregates.Helpdesk.Core.Marten;
using PocMarten.Api.Aggregates.Helpdesk.Dto;
using PocMarten.Api.Aggregates.Helpdesk.Models;
using PocMarten.Api.Aggregates.Helpdesk.Service.Models;
using Swashbuckle.AspNetCore.Annotations;
using static PocMarten.Api.Aggregates.Helpdesk.Service.IncidentService;
using static PocMarten.Api.Aggregates.Helpdesk.Core.Http.ETagExtensions;
using PocMarten.Api.Aggregates.Helpdesk.Models.GetIncidentShortInfo;
using Marten.Pagination;
using Marten.Internal.Sessions;
using PocMarten.Api.Aggregates.Helpdesk.Models.GetIncidentHistory;

namespace PocMarten.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelpdeskController : ControllerBase
    {
        private readonly IDocumentSession _documentSession;
        private readonly IQuerySession _querySession;

        public HelpdeskController(IDocumentSession documentSession, IQuerySession querySession)
        {
            _documentSession = documentSession;
            _querySession = querySession;
        }

        [HttpPost]
        [Route("customers/{customerId:guid}/incidents/")]
        [SwaggerOperation(Tags = new[] { "Customers" })]
        public async Task<IActionResult> IncidentLogged(
            [FromRoute] Guid customerId,
            [FromBody] LogIncidentRequest body,
            CancellationToken cancellationToken)
        {
            var (contact, description) = body;
            var incidentId = CombGuidIdGeneration.NewGuid();

            await _documentSession.Add<Incident>(incidentId,
                Handle(new LogIncident(incidentId, customerId, contact, description, customerId)), cancellationToken)
                .ConfigureAwait(false);

            return Created($"/api/incidents/{incidentId}", incidentId);
        }

        [HttpPost]
        [Route("agents/{agentId:guid}/incidents/{incidentId:guid}/category")]
        [SwaggerOperation(Tags = new[] { "Agents" })]
        public async Task IncidentCategorised(
            [FromRoute] Guid incidentId,
            [FromRoute] Guid agentId,
            [FromHeader(Name = "If-Match")] string eTag,
            [FromBody] CategoriseIncidentRequest body,
            CancellationToken cancellationToken)
                => await _documentSession
                    .GetAndUpdate<Incident>(
                    incidentId,
                    ToExpectedVersion(eTag),
                    current => Handle(current, new CategoriseIncident(incidentId, body.Category, agentId)), cancellationToken)
                    .ConfigureAwait(false);

        [HttpPost]
        [Route("agents/{agentId:guid}/incidents/{incidentId:guid}/priority")]
        [SwaggerOperation(Tags = new[] { "Agents" })]
        public async Task IncidentPrioritised(
            [FromRoute] Guid incidentId,
            [FromRoute] Guid agentId,
            [FromHeader(Name = "If-Match")] string eTag,
            PrioritiseIncidentRequest body,
            CancellationToken cancellationToken)
            => await _documentSession
                .GetAndUpdate<Incident>(
                    incidentId,
                    ToExpectedVersion(eTag),
                    current => Handle(current, new PrioritiseIncident(incidentId, body.Priority, agentId)), cancellationToken)
                .ConfigureAwait(false);

        [HttpPost]
        [Route("agents/{agentId:guid}/incidents/{incidentId:guid}/assign")]
        [SwaggerOperation(Tags = new[] { "Agents" })]
        public async Task AgentAssignedToIncident(
            [FromRoute] Guid incidentId,
            [FromRoute] Guid agentId,
            [FromHeader(Name = "If-Match")] string eTag,
            CancellationToken cancellationToken)
            => await _documentSession
                .GetAndUpdate<Incident>(
                    incidentId,
                    ToExpectedVersion(eTag),
                    current => Handle(current, new AssignAgentToIncident(incidentId, agentId)), cancellationToken)
                .ConfigureAwait(false);



        [HttpPost]
        [Route("customers/{customerId:guid}/incidents/{incidentId:guid}/responses")]
        [SwaggerOperation(Tags = new[] { "Customers" })]
        public async Task CustomerRespondedToIncident(
            [FromRoute] Guid incidentId,
            [FromRoute] Guid customerId,
            [FromHeader(Name = "If-Match")] string eTag,
            RecordCustomerResponseToIncidentRequest body,
            CancellationToken cancellationToken)
            => await _documentSession
                .GetAndUpdate<Incident>(
                    incidentId,
                    ToExpectedVersion(eTag),
                    current => Handle(
                        current,
                        new RecordCustomerResponseToIncident(incidentId,
                            new IncidentResponse.FromCustomer(customerId, body.Content))), cancellationToken)
                .ConfigureAwait(false);

        [HttpPost]
        [Route("agents/{agentId:guid}/incidents/{incidentId:guid}/responses")]
        [SwaggerOperation(Tags = new[] { "Agents" })]
        public async Task AgentRespondedToIncident(
            [FromHeader(Name = "If-Match")] string eTag,
            [FromRoute] Guid incidentId,
            [FromRoute] Guid agentId,
            RecordAgentResponseToIncidentRequest body,
            CancellationToken cancellationToken)
        {
            var (content, visibleToCustomer) = body;

            await _documentSession
                .GetAndUpdate<Incident>(
                    incidentId,
                    ToExpectedVersion(eTag),
                    current => Handle(
                        current,
                        new RecordAgentResponseToIncident(incidentId,
                            new IncidentResponse.FromAgent(agentId, content, visibleToCustomer))), cancellationToken)
                .ConfigureAwait(false);
        }

        [HttpPost]
        [Route("agents/{agentId:guid}/incidents/{incidentId:guid}/resolve")]
        [SwaggerOperation(Tags = new[] { "Agents" })]
        public async Task IncidentResolved(
            [FromRoute] Guid incidentId,
            [FromRoute] Guid agentId,
            [FromHeader(Name = "If-Match")] string eTag,
            ResolveIncidentRequest body,
            CancellationToken cancellationToken)
            => await _documentSession
                .GetAndUpdate<Incident>(
                    incidentId,
                    ToExpectedVersion(eTag),
                    current => Handle(current, new ResolveIncident(incidentId, body.Resolution, agentId)), cancellationToken)
                .ConfigureAwait(false);

        [HttpPost]
        [Route("customers/{customerId:guid}/incidents/{incidentId:guid}/acknowledge")]
        [SwaggerOperation(Tags = new[] { "Customers" })]
        public async Task ResolutionAcknowledgedByCustomer(
            [FromRoute] Guid incidentId,
            [FromRoute] Guid customerId,
            [FromHeader(Name = "If-Match")] string eTag,
            CancellationToken cancellationToken)
            => await _documentSession
                        .GetAndUpdate<Incident>(
                            incidentId,
                            ToExpectedVersion(eTag),
                            current => Handle(current, new AcknowledgeResolution(incidentId, customerId)), cancellationToken)
                        .ConfigureAwait(false);

        [HttpPost]
        [Route("agents/{agentId:guid}/incidents/{incidentId:guid}/close")]
        [SwaggerOperation(Tags = new[] { "Customers" })]
        public async Task<IActionResult> IncidentClosed(
            [FromRoute] Guid incidentId,
            [FromRoute] Guid agentId,
            [FromHeader(Name = "If-Match")] string eTag,
            CancellationToken cancellationToken)
        {
            await _documentSession.GetAndUpdate<Incident>(
                incidentId,
                ToExpectedVersion(eTag),
                current => Handle(current, new CloseIncident(incidentId, agentId)), cancellationToken); ;

            return Ok();
        }

        [HttpGet]
        [Route("customers/{customerId:guid}/incidents")]
        [SwaggerOperation(Tags = new[] { "Incident" })]
        public async Task<IActionResult> GetIncidentShortInfo(
            [FromRoute] Guid customerId,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            CancellationToken cancellationToken)
        {
            var result = await _querySession.Query<IncidentShortInfo>()
                 .Where(x => x.CustomerId == customerId)
                 .ToPagedListAsync(pageNumber ?? 1, pageSize ?? 10, cancellationToken)
                 .ConfigureAwait(false);

            return Ok(result);
        }

        [HttpGet]
        [Route("incidents/{incidentId:guid}/history")]
        [SwaggerOperation(Tags = new[] { "Incident" })]
        public async Task GetIncidentHistory(
            [FromRoute] Guid incidentId,
            CancellationToken cancellationToken)
            => await _querySession
                .Query<IncidentHistory>()
                .Where(i => i.IncidentId == incidentId)
                .WriteArray(this.HttpContext)
                .ConfigureAwait(false);
    }
}
