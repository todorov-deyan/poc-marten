using FluentValidation;
using PocMarten.Api.Aggregates.BankAccount.Commands;
using PocMarten.Api.Aggregates.BankAccount.ModelDto;

namespace PocMarten.Api.Aggregates.BankAccount.Validators
{
    public class AccountCreateRequestValidator : AbstractValidator<AccountCreateRequest>
    {
        public AccountCreateRequestValidator()
        {
            RuleFor(x => x.Balance).GreaterThan(0);
            RuleFor(x => x.Owner).NotNull().NotEmpty();
        }
    }
}
