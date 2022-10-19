using FluentValidation;
using PocMarten.Api.Aggregates.BankAccount.Commands;

namespace PocMarten.Api.Aggregates.BankAccount.Validators
{
    public class AccountCreateCommandValidator : AbstractValidator<AccountCreateCommand>
    {
        public AccountCreateCommandValidator()
        {
            RuleFor(x => x.CreateRequest.Balance).GreaterThan(0);
            RuleFor(x => x.CreateRequest.Owner).NotNull().NotEmpty();
        }
    }
}
