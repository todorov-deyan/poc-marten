using Marten;
using PocMarten.Api.Aggregates.BankAccount.Model;
using PocMarten.Api.Repository;

namespace PocMarten.Api.Aggregates.BankAccount.Repository
{
    public class BankAccountRepository : MartenRepository<Account>
    {
        private readonly IDocumentSession _session;

        public BankAccountRepository(IDocumentSession session) : base(session)
        {
            _session = session;
        }

        public Task<IReadOnlyList<Account>> GetAccounts(IEnumerable<Guid> accounts)
        {
            return _session.LoadManyAsync<Account>(accounts);
        }
    }
}
