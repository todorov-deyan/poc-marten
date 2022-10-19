using FluentValidation;
using PocMarten.Api.Aggregates.BankAccount.Commands;
using PocMarten.Api.Common.Validators;

namespace PocMarten.Api.Aggregates.BankAccount.Validators
{
    public class AccountOperationCommandValidator : AbstractValidator<AccountOperationCommand>
    {
        public AccountOperationCommandValidator()
        {
            RuleFor(x => x.OperationRequest.AccountId).SetValidator(new GuidValidator());
            RuleFor(x => x.OperationRequest.Balance).GreaterThan(0);
            RuleFor(x => x.OperationRequest.OperationType).IsInEnum();
        }
    }
}
