using MediatR;

namespace PocMarten.Api.Common.CQRS
{
    public interface ICommandRequest <out TResponse> : IRequest<TResponse>
    {
    }
}
