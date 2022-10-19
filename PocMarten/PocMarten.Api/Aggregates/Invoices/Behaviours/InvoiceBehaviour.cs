using MediatR;

namespace PocMarten.Api.Aggregates.Invoices.Behaviours
{
    public class InvoiceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<InvoiceBehaviour<TRequest, TResponse>> _logger;

        public InvoiceBehaviour(ILogger<InvoiceBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Before");

            return await next();
        }
    }
}
