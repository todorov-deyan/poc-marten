using Ardalis.Result;
using PocMarten.Api.Aggregates.BankAccount.Model;
using PocMarten.Api.Aggregates.BankAccount.Repository;
using PocMarten.Api.Common.CQRS;

namespace PocMarten.Api.Aggregates.BankAccount.Queries
{
    public record GetAccountByIdQuery(Guid Id) : IQueryRequest<Result<Account>>;



    public class GetAccountByIdQueryHandlers : IQueryHandler<GetAccountByIdQuery, Result<Account>>
    {
        private readonly BankAccountRepository _repository;

        public GetAccountByIdQueryHandlers(BankAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Account>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.Find(request.Id, cancellationToken);
            if (result is null)
                return Result.NotFound();

            return Result<Account>.Success(result);
        }
    }
}
