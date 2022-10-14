using PocMarten.Api.Aggregates.BankAccount.Events;
using PocMarten.Api.Aggregates.BankAccount.Model;
using PocMarten.Api.Aggregates.BankAccount.Repository;
using PocMarten.Api.Common.EventSourcing;
using System.Threading;

namespace PocMarten.Api.Aggregates.BankAccount.Service
{
    public class TransferService
    {
        private readonly BankTransactionRepository _bankTransactionRepository;
        private readonly BankAccountRepository _accountRepository;

        public TransferService(BankTransactionRepository bankTransactionRepository, BankAccountRepository accountRepository)
        {
            _bankTransactionRepository = bankTransactionRepository;
            _accountRepository = accountRepository;
        }

        public async Task<bool> DoTransfer(Guid fromAccountId, Guid toAccountId, decimal amount, CancellationToken cancellationToken = defaul)
        {
            Account? fromAccount = await _accountRepository.Find(fromAccountId, cancellationToken);
            if (fromAccount is null)
                return false;


            Account? toAccount = await _accountRepository.Find(toAccountId, cancellationToken);
            if (toAccount is null)
                return false;


            TransactionStarted startTransaction = new()
            {
                AccountId = toAccount.Id,
                Owner = toAccount.Owner
            };

            Transaction transaction = new(startTransaction)
            {
                Id = Guid.NewGuid(),
                From = fromAccount.Id,
                To = toAccount.Id,
                Amount = amount,
                Description = "Money Transfer"
            };

            List<IEventState> events = new()
            {
                new TransactionProccessed(),
                new TransactionComplated()
            };

            toAccount.Transactions.Add(transaction);

            //_transactionRepository.Add(transaction, events, cancellationToken);


            //await _repository.Update(toAccount, events, cancellationToken);

            return true;
        }
    }
}
