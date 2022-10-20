using FluentValidation;
using PocMarten.Api.Aggregates.Invoices.Handlers;

namespace PocMarten.Api.Aggregates.Invoices.Validators
{
    public class InvoiceCreateCommandValidator : AbstractValidator<AddAmountCommand>
    {
        public InvoiceCreateCommandValidator()
        {
            RuleFor(x => x.amount).NotNull().NotEmpty();
            RuleFor(x => x.amount).GreaterThan(0);
        }
    }
}
