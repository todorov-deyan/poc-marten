using MediatR;

namespace PocMarten.Api.Aggregates.Weather.Behaviours
{
    public class WeatherBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<WeatherBehaviour<TRequest, TResponse>> _logger;

        public WeatherBehaviour(ILogger<WeatherBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Before");

            var result = await next();

            _logger.LogInformation($"After");
            
            return result;
        }
        
    }
}
