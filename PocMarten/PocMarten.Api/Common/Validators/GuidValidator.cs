using FluentValidation;

namespace PocMarten.Api.Common.Validators
{
    public class GuidValidator : AbstractValidator<Guid>
    {
        public GuidValidator()
        {
            RuleFor(x => x).NotEmpty();
        }
    }
}
