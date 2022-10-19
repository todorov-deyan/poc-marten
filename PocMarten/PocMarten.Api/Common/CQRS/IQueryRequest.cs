using MediatR;

namespace PocMarten.Api.Common.CQRS
{
    public interface IQueryRequest<out TResponse> : IRequest<TResponse>
    {
    }
}
