using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PocMarten.Api.Aggregates.BankAccount.Events;
using PocMarten.Api.Aggregates.BankAccount.Model;
using PocMarten.Api.Aggregates.BankAccount.ModelDto;
using PocMarten.Api.Aggregates.BankAccount.Repository;
using PocMarten.Api.Aggregates.Weather.Events;
using PocMarten.Api.Aggregates.Weather.Model;
using PocMarten.Api.Aggregates.Weather.Repository;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class BankAccountController : Controller
    {
        private readonly BankAccountRepository _repository;
        private readonly ILogger<BankAccountController> _logger;

        public BankAccountController(BankAccountRepository repository, ILogger<BankAccountController> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        [HttpGet("{accountId:guid}", Name = "GetAccount")]
        public async Task<ActionResult<Account>> Get(Guid accountId, CancellationToken cancellationToken = default)
        {
            var result = await _repository.Find(accountId, cancellationToken);

            if (result is null)
                return NotFound();

            return Ok(result);
        }
        

        [HttpPost(Name = "CreateAccount")]
        public async Task<ActionResult> Post(AccountDto accountCreate, CancellationToken cancellationToken = default)
        {
            Account newAccount = new()
            {
                Id = Guid.NewGuid()
            };

            AccountCreated accountCreated = new()
            {
                AccountId = newAccount.Id,
                Owner = accountCreate.Owner,
                StartingBalance = accountCreate.Balance,
                Description = accountCreate.Description
            };

            List<IEventState> events = new()
            {
                accountCreated
            };

            await _repository.Add(newAccount, events, cancellationToken);

            return CreatedAtAction("Get", new { accountId = newAccount.Id }, accountCreated);
        }
    }
}
