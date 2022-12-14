using Marten;
using PocMarten.Api.Aggregates.BankAccount.Model;
using PocMarten.Api.Common.EventSourcing;
using PocMarten.Api.Common.Repository;

namespace PocMarten.Api.Aggregates.BankAccount.Repository
{
    public class BankAccountRepository : MartenRepository<Account>
    {
        private readonly IDocumentSession _session;

        public BankAccountRepository(IDocumentSession session) : base(session)
        {
            _session = session;
        }
    }
}
