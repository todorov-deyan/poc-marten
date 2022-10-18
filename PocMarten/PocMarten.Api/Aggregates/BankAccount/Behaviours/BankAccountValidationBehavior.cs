using MediatR;
using PocMarten.Api.Aggregates.Weather.Behaviours;

namespace PocMarten.Api.Aggregates.BankAccount.Behaviours
{
    public class BankAccountValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<BankAccountValidationBehavior<TRequest, TResponse>> _logger;

        public BankAccountValidationBehavior(ILogger<BankAccountValidationBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }
        
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            return await next();
        }
    }
}
