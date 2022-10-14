using Microsoft.AspNetCore.Mvc;
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
        private readonly ILogger<BankAccountOperationsController> _logger;


        public BankAccountOperationsController(BankAccountRepository repository,
                                               ILogger<BankAccountOperationsController> logger)
        {
            _repository = repository;
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
                AccountOperationTypeDto.Close => new AccountClosed
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

        [HttpPost("/BankAccountTransfer", Name = "AccountTransfer")]
        public async Task<ActionResult> Post(Guid fromAccountId, Guid toAccountId, decimal amount, CancellationToken cancellationToken = default)
        {
            Account? fromAccount = await _repository.Find(fromAccountId, cancellationToken);
            if (fromAccount is null)
                return NotFound();

            Account? toAccount = await _repository.Find(toAccountId, cancellationToken);
            if (toAccount is null)
                return NotFound();


            TransactionStarted startTransaction = new()
            {
                AccountId = toAccount.Id,
                Description = $"Start Money transfer amount: {amount} from account: {fromAccountId} to account:{toAccountId}"
            };

            TransactionProccessed proccesedTransaction = new()
            {

                From = fromAccount.Id,
                To = toAccount.Id,
                Amount = amount,
                Description = $"Process Money transfer amount: {amount} from account: {fromAccountId} to account:{toAccountId}"
            };

            TransactionComplated complateTransaction = new()
            {
                AccountId = toAccount.Id,
                Description = $"Complated Money transfer amount: {amount} from account: {fromAccountId} to account:{toAccountId}"

            };

            List<IEventState> events = new()
            {
                startTransaction,
                proccesedTransaction,
                complateTransaction
            };

            await _repository.Update(toAccountId, events, cancellationToken);

            return CreatedAtAction("Get", "BankAccount", new { accountId = toAccountId }, new { accountId = toAccountId });

        }
    }
}
