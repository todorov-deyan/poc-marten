using MediatR;
using Microsoft.AspNetCore.Mvc;
using PocMarten.Api.Aggregates.BankAccount.Commands;
using PocMarten.Api.Aggregates.BankAccount.Events;
using PocMarten.Api.Aggregates.BankAccount.Model;
using PocMarten.Api.Aggregates.BankAccount.ModelDto;
using PocMarten.Api.Aggregates.BankAccount.Repository;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Controllers
{
    [ApiController]
    [Route("[controller]/{accountId:guid}/operation")]
    public class BankAccountOperationsController : Controller
    {
        private readonly BankAccountRepository _repository;
        private readonly IMediator _mediator;
        private readonly ILogger<BankAccountOperationsController> _logger;


        public BankAccountOperationsController(BankAccountRepository repository, IMediator mediator,
                                               ILogger<BankAccountOperationsController> logger)
        {
            _repository = repository;
            _mediator = mediator;
            _logger = logger;
        }


        [HttpPost(Name = "AccountOperation")]
        public async Task<ActionResult> Post(AccountOperationRequest operationRequest,
                                             CancellationToken cancellationToken = default)
        {
            if (operationRequest is null)
                return BadRequest();

            var accountId = await _mediator.Send(new AccountOperationCommand(operationRequest), cancellationToken);

            return CreatedAtAction("Get", "BankAccount", new { accountId = accountId }, new { accountId = accountId, operationType = operationRequest.OperationType });
        }


        [HttpPost("/BankAccountTransfer", Name = "AccountTransfer")]
        public async Task<ActionResult> Post(AccountTransferRequest transferRequest, CancellationToken cancellationToken = default)
        {
            var accountId = await _mediator.Send(new AccountTransferCommand(transferRequest), cancellationToken);
            if (!accountId.IsSuccess)
                return BadRequest();

            return CreatedAtAction("Get", "BankAccount", new { accountId = accountId }, new { accountId = accountId });
        }
    }
}
