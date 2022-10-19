using MediatR;

namespace PocMarten.Api.Common.CQRS
{
    public interface IQueryHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IQueryRequest<TResponse>
    {
    }

}
