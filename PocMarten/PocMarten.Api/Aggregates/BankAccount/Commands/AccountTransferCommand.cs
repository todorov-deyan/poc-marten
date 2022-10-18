using Ardalis.Result;
using MediatR;
using PocMarten.Api.Aggregates.BankAccount.Events;
using PocMarten.Api.Aggregates.BankAccount.Model;
using PocMarten.Api.Aggregates.BankAccount.ModelDto;
using PocMarten.Api.Aggregates.BankAccount.Repository;
using PocMarten.Api.Common.CQRS;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BankAccount.Commands
{
    public record AccountTransferCommand(AccountTransferRequest TransferRequest) : ICommandRequest<Result<Guid>>;

    public class AccountTransferCommandHandler : ICommandHandler<AccountTransferCommand, Result<Guid>>
    {
        private readonly BankAccountRepository _repository;

        public AccountTransferCommandHandler(BankAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Guid>> Handle(AccountTransferCommand request, CancellationToken cancellationToken)
        {
            Account? fromAccount = await _repository.Find(request.TransferRequest.FromAccountId, cancellationToken);
            if (fromAccount is null)
                return Result.NotFound("Invalid source account.");

            Account? toAccount = await _repository.Find(request.TransferRequest.ToAccountId, cancellationToken);
            if (toAccount is null)
                return Result.NotFound("Invalid destination account.");


            TransactionStarted startTransaction = new()
            {
                AccountId = toAccount.Id,
                Description = $"Start Money transfer amount: {request.TransferRequest.Amount} from account: {request.TransferRequest.FromAccountId} to account:{request.TransferRequest.ToAccountId}"
            };

            TransactionProccessed proccesedTransaction = new()
            {

                From = fromAccount.Id,
                To = toAccount.Id,
                Amount = request.TransferRequest.Amount,
                Description = $"Process Money transfer amount: {request.TransferRequest.Amount} from account: {request.TransferRequest.FromAccountId} to account:{request.TransferRequest.ToAccountId}"
            };

            TransactionComplated complateTransaction = new()
            {
                AccountId = toAccount.Id,
                Description = $"Complated Money transfer amount: {request.TransferRequest.Amount} from account: {request.TransferRequest.FromAccountId} to account:{request.TransferRequest.ToAccountId}"

            };

            List<IEventState> events = new()
            {
                startTransaction,
                proccesedTransaction,
                complateTransaction
            };

            await _repository.Update(request.TransferRequest.ToAccountId, events, cancellationToken);


            return Result.Success(toAccount.Id);
        }
    }
}
