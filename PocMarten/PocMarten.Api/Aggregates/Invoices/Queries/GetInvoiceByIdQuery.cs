using Ardalis.Result;
using MediatR;
using PocMarten.Api.Aggregates.Invoices.Models;
using PocMarten.Api.Aggregates.Invoices.Repository;
using PocMarten.Api.Common.CQRS;

namespace PocMarten.Api.Aggregates.Invoices.Queries
{
    public record GetInvoiceByIdQuery(Guid Id) : IQueryRequest<Result<InvoiceModel>>;

    public class GetInvoiceByIdQueryHandler : IQueryHandler<GetInvoiceByIdQuery, Result<InvoiceModel>>
    {
        private readonly InvoiceRepository _invoiceRepository;

        public GetInvoiceByIdQueryHandler(InvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<Result<InvoiceModel>> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
           var result = await _invoiceRepository.Find(request.Id, cancellationToken);

            if (result == null)
                return Result.NotFound();

            return Result.Success(result);
        }
    }
}

