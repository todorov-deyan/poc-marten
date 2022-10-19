using MediatR;
using PocMarten.Api.Aggregates.Invoices.Models;
using PocMarten.Api.Aggregates.Invoices.Repository;
using static PocMarten.Tests.InvoiceTests;

namespace PocMarten.Api.Aggregates.Invoices.Queries
{
    public record GetInvoiceByIdQuery(Guid Id) : IRequest<InvoiceModel>;
    public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceModel>
    {
        private readonly InvoiceRepository _invoiceRepository;

        public GetInvoiceByIdQueryHandler(InvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<InvoiceModel> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
           var result = await _invoiceRepository.Find(request.Id, cancellationToken);
           return result;
        }
    }
}

