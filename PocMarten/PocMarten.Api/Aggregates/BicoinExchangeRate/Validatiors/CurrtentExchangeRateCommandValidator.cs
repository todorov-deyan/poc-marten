using FluentValidation;
using PocMarten.Api.Aggregates.BicoinExchangeRate.Commands;

namespace PocMarten.Api.Aggregates.BicoinExchangeRate.Validatiors
{
    public class CurrtentExchangeRateCommandValidator : AbstractValidator<CurrentExchangeRateCommand>
    {
        public CurrtentExchangeRateCommandValidator()
        {
            RuleFor(x => x.currentExchangeRate).NotEmpty();
        }
    }
}
