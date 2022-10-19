using MediatR;

namespace PocMarten.Api.Aggregates.Weather.Behaviours
{
    public class WeatherLoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<WeatherLoggingBehaviour<TRequest, TResponse>> _logger;

        public WeatherLoggingBehaviour(ILogger<WeatherLoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {typeof(TRequest).Name}");

            var result = await next();

            _logger.LogInformation($"Handled {typeof(TResponse).Name}");
            
            return result;
        }
        
    }
}
