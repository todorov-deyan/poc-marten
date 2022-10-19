using FluentValidation;
using PocMarten.Api.Aggregates.BankAccount.Commands;
using PocMarten.Api.Common.Validators;

namespace PocMarten.Api.Aggregates.BankAccount.Validators
{
    public class AccountTransferCommandValidator : AbstractValidator<AccountTransferCommand>
    {
        public AccountTransferCommandValidator()
        {
            RuleFor(x => x.TransferRequest.ToAccountId).SetValidator(new GuidValidator());
            RuleFor(x => x.TransferRequest.FromAccountId).SetValidator(new GuidValidator());
            RuleFor(x => x.TransferRequest.Amount).GreaterThan(0);
        }
    }
}
