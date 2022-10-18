using MediatR;
using Microsoft.AspNetCore.Mvc;
using PocMarten.Api.Aggregates.BankAccount.Commands;
using PocMarten.Api.Aggregates.BankAccount.Events;
using PocMarten.Api.Aggregates.BankAccount.Model;
using PocMarten.Api.Aggregates.BankAccount.ModelDto;
using PocMarten.Api.Aggregates.BankAccount.Queries;
using PocMarten.Api.Aggregates.BankAccount.Repository;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class BankAccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BankAccountController> _logger;

        public BankAccountController(IMediator mediator, ILogger<BankAccountController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }


        [HttpGet("{accountId:guid}", Name = "GetAccount")]
        public async Task<ActionResult<Account>> Get(Guid accountId, CancellationToken cancellationToken = default)
        {
            var account = await _mediator.Send(new GetAccountByIdQuery(accountId), cancellationToken);
            if(!account.IsSuccess)
                return NotFound();

            return Ok(account);
        }


        [HttpPost(Name = "CreateAccount")]
        public async Task<ActionResult> Post(AccountCreateRequest createAccount, CancellationToken cancellationToken = default)
        {
            var account =  await _mediator.Send(new AccountCreateCommand(createAccount), cancellationToken);
            if (!account.IsSuccess)
                return BadRequest();

            return CreatedAtAction("Get", new { accountId = account.Value.Id }, account.Value);
        }
    }
}
