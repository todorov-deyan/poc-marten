using Ardalis.Result;
using PocMarten.Api.Aggregates.BankAccount.Events;
using PocMarten.Api.Aggregates.BankAccount.Model;
using PocMarten.Api.Aggregates.BankAccount.ModelDto;
using PocMarten.Api.Aggregates.BankAccount.Repository;
using PocMarten.Api.Common.CQRS;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Commands
{
    public record AccountCreateCommand(AccountCreateRequest createRequest) : ICommandRequest<Result<Account>>;


    public class AccountCreateCommandHandler : ICommandHandler<AccountCreateCommand, Result<Account>>
    {
        private readonly BankAccountRepository _repository;

        public AccountCreateCommandHandler(BankAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Account>> Handle(AccountCreateCommand request, CancellationToken cancellationToken)
        {
            AccountCreated accountCreated = new()
            {
                Owner = request.createRequest.Owner,
                StartingBalance = request.createRequest.Balance,
                IsOverdraftAllowed = true,
                Description = request.createRequest.Description
            };

            var newAccount = Account.Create(accountCreated);

            await _repository.Add(newAccount, new List<IEventState>() { accountCreated }, cancellationToken: cancellationToken);
            
            return Result.Success(newAccount);
        }
    }
}
