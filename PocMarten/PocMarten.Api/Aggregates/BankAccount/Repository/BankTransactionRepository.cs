using Marten;
using PocMarten.Api.Aggregates.BankAccount.Model;
using PocMarten.Api.Common.Repository;

namespace PocMarten.Api.Aggregates.BankAccount.Repository
{
    public class BankTransactionRepository : MartenRepository<Transaction>
    {
        private readonly IDocumentSession _session;

        public BankTransactionRepository(IDocumentSession session) : base(session)
        {
            _session = session;
        }
    }

}
