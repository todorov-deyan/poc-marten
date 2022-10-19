using Ardalis.Result;
using PocMarten.Api.Aggregates.BankAccount.Events;
using PocMarten.Api.Aggregates.BankAccount.Model;
using PocMarten.Api.Aggregates.BankAccount.ModelDto;
using PocMarten.Api.Aggregates.BankAccount.Repository;
using PocMarten.Api.Common.CQRS;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Commands
{
    public record AccountOperationCommand(AccountOperationRequest OperationRequest) : ICommandRequest<Result<Guid>>;


    public class AccountOperationCommandHandler : ICommandHandler<AccountOperationCommand, Result<Guid>>
    {
        private readonly BankAccountRepository _repository;

        public AccountOperationCommandHandler(BankAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Guid>> Handle(AccountOperationCommand request, CancellationToken cancellationToken)
        {
            Account? account = await _repository.Find(request.OperationRequest.AccountId, cancellationToken);
            if (account is null)
                return Result.NotFound();
            

            IEventState? bankOperation = request.OperationRequest.OperationType switch
            {
                AccountOperationType.Withdraw => new AccountWithdrawed
                {
                    Amount = request.OperationRequest.Balance,
                    Description = request.OperationRequest.Description
                },
                AccountOperationType.Deposit => new AccountDebited
                {
                    Amount = request.OperationRequest.Balance,
                    Description = request.OperationRequest.Description
                },
                AccountOperationType.Close => new AccountClosed
                {
                    ClosingBalance = request.OperationRequest.Balance,
                    Description = request.OperationRequest.Description
                },
                AccountOperationType.None => throw new InvalidOperationException(),
                _ => throw new InvalidOperationException()
            };
            
            List<IEventState> events = new() { bankOperation };

            await _repository.Update(account.Id, events, cancellationToken);
            

            return Result.Success(account.Id);
        }
    }
}
