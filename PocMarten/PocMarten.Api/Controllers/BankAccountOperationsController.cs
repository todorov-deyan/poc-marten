using Microsoft.AspNetCore.Mvc;
using PocMarten.Api.Aggregates.BankAccount.Events;
using PocMarten.Api.Aggregates.BankAccount.Model;
using PocMarten.Api.Aggregates.BankAccount.ModelDto;
using PocMarten.Api.Aggregates.BankAccount.Repository;
using PocMarten.Api.Aggregates.Weather.Events;
using PocMarten.Api.Aggregates.Weather.Model;
using PocMarten.Api.Aggregates.Weather.Respository;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Controllers
{
    [ApiController]
    [Route("[controller]/{accountId:guid}/operation")]
    public class BankAccountOperationsController : Controller
    {
        private readonly BankAccountRepository _repository;
        private readonly ILogger<BankAccountOperationsController> _logger;


        public BankAccountOperationsController(BankAccountRepository repository, ILogger<BankAccountOperationsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        [HttpPost(Name = "CreateOperation")]
        public async Task<ActionResult> Post(Guid accountId, AccountDto accountDetails, AccountOperationTypeDto operationType,CancellationToken cancellationToken = default)
        {
            Account? account = await _repository.Find(accountId, cancellationToken);
            if (account is null)
                return NotFound();

            List<IEventState> events = new();

            switch (operationType)
            {
                case AccountOperationTypeDto.Credit:
                    events.Add(new AccountCredited
                                    {
                                        Amount = accountDetails.Balance,
                                        Description = accountDetails.Description
                                    });
                    break;
                case AccountOperationTypeDto.Deposit:
                    events.Add(new AccountDebited
                                    {
                                        Amount = accountDetails.Balance,
                                        Description = accountDetails.Description
                                    });
                    break;
                case AccountOperationTypeDto.Close:
                    events.Add(new AccountClosed()
                    {
                        ClosingBalance = accountDetails.Balance,
                        Description = accountDetails.Description
                    });
                    break;
                default:
                    return BadRequest();
            };

            await _repository.Update(account.Id, events, cancellationToken);

            return CreatedAtAction("Get", "BankAccount", new { accountId = accountId }, new { accountId = accountId });
        }

    }
}
