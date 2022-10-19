using FluentValidation;
using MediatR;
using PocMarten.Api.Common.CQRS;

namespace PocMarten.Api.Aggregates.BankAccount.Behaviours
{
    public class BankAccountValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
                                                                      where TRequest : ICommandRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<BankAccountValidationBehavior<TRequest, TResponse>> _logger;

        public BankAccountValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<BankAccountValidationBehavior<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }
        
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if(!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);
            var errors = _validators.Select(x => x.Validate(context))
                                                                .SelectMany(x => x.Errors)
                                                                .Where(x => x != null);

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }


            return await next();
        }
    }
}
