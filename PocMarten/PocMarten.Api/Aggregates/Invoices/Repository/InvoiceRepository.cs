using Marten;
using PocMarten.Api.Aggregates.Invoices.Models;
using PocMarten.Api.Common.Repository;

namespace PocMarten.Api.Aggregates.Invoices.Repository
{
    public class InvoiceRepository : MartenRepository<InvoiceModel>
    {
        public InvoiceRepository(IDocumentSession documentSession) : base(documentSession)
        {
        }
    }
}
