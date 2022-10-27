using MediatR;

namespace PocMarten.Api.Aggregates.BicoinExchangeRate.Behaviours
{
    public class ExchangeRateBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<ExchangeRateBehaviour<TRequest, TResponse>> _logger;

        public ExchangeRateBehaviour(ILogger<ExchangeRateBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Hadling {typeof(TRequest).Name}");

            var result = await next();

            _logger.LogInformation($"Hadled {typeof(TResponse).Name}");

            return result;
        }
    }
}
