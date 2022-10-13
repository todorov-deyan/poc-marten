using Marten;
using PocMarten.Api.Repository;
using PocMarten.Api.Aggregates.Invoices.Models;

namespace PocMarten.Api.Aggregates.Invoices.Repository

{
    public class InvoiceRepository : MartenRepository<Invoice>
    {
        public InvoiceRepository(IDocumentSession documentSession) : base(documentSession)
        {
        }
    }
}
