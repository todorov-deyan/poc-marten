using Marten;
using Marten.Schema.Identity;

using Microsoft.AspNetCore.Mvc;

using PocMarten.Api.Aggregates.Helpdesk.Core.Marten;
using PocMarten.Api.Aggregates.Helpdesk.Dto;
using PocMarten.Api.Aggregates.Helpdesk.Models;

using static PocMarten.Api.Aggregates.Helpdesk.Service.IncidentService;

namespace PocMarten.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelpdeskController : ControllerBase
    {
        private readonly IDocumentSession _documentSession;

        public HelpdeskController(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        [HttpPost]
        [Route("api/customers/{customerId:guid}/incidents/")]
        public async Task<IActionResult> Post([FromRoute] Guid customerId, [FromBody] LogIncidentRequest body, CancellationToken cancellationToken)
        {
            var (contact, description) = body;
            var incidentId = CombGuidIdGeneration.NewGuid();

            await _documentSession.Add<Incident>(incidentId,
                Handle(new Aggregates.Helpdesk.Service.Models.LogIncident(incidentId, customerId, contact, description, customerId)), cancellationToken);

            return Created($"/api/incidents/{incidentId}", incidentId);
        }
    }
}
