using Microsoft.AspNetCore.Mvc;
using PocMarten.Api.Aggregates.BankAccount.Events;
using PocMarten.Api.Aggregates.BankAccount.Model;
using PocMarten.Api.Aggregates.BankAccount.ModelDto;
using PocMarten.Api.Aggregates.BankAccount.Repository;
using PocMarten.Api.Aggregates.BankAccount.Service;
using PocMarten.Api.Common.EventSourcing;
using System.Security.Principal;

namespace PocMarten.Api.Controllers
{
    [ApiController]
    [Route("[controller]/{accountId:guid}/operation")]
    public class BankAccountOperationsController : Controller
    {
        private readonly BankAccountRepository _repository;
        private readonly TransferService _transferService;

        private readonly ILogger<BankAccountOperationsController> _logger;


        public BankAccountOperationsController(BankAccountRepository repository,
                                               TransferService transferService,
                                               ILogger<BankAccountOperationsController> logger)
        {
            _repository = repository;
            _transferService = transferService;
            _logger = logger;
        }


        [HttpPost(Name = "AccountOperation")]
        public async Task<ActionResult> Post(Guid accountId,
                                             AccountDto accountDetails, 
                                             AccountOperationTypeDto operationType,
                                             CancellationToken cancellationToken = default)
        {

            if (accountDetails is null)
                return BadRequest();

            Account? account = await _repository.Find(accountId, cancellationToken);
            if (account is null)
                return NotFound();


            IEventState? bankOperation = operationType switch
            {
                AccountOperationTypeDto.Withdraw => new AccountWithdrawed
                {
                    Amount = accountDetails.Balance,
                    Description = accountDetails.Description
                },
                AccountOperationTypeDto.Deposit => new AccountDebited
                {
                    Amount = accountDetails.Balance,
                    Description = accountDetails.Description
                },
                AccountOperationTypeDto.Close => new AccountClosed()
                {
                    ClosingBalance = accountDetails.Balance,
                    Description = accountDetails.Description
                },
                AccountOperationTypeDto.None => throw new InvalidOperationException(),
                _ => throw new InvalidOperationException()
            };
            
            List<IEventState> events = new() { bankOperation };

            await _repository.Update(account.Id, events, cancellationToken);

            return CreatedAtAction("Get", "BankAccount", new { accountId = accountId }, new { accountId = accountId, operationType = operationType, details = bankOperation });
        }

        [HttpPost(Name = "AccountTransfer")]
        public async Task<ActionResult> Post(Guid fromAccountId, Guid toAccountId, decimal amount, CancellationToken cancellationToken = default)
        {

            bool result = _transferService.DoTransfer(fromAccountId, toAccountId, amount);

            return CreatedAtAction("Get", "BankAccount", new { accountId = accountId }, new { accountId = accountId });
        }
    }
}
