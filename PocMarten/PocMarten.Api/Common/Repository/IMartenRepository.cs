using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Common.Repository
{
    public interface IMartenRepository<T> where T : class, IAggregate
    {
        Task<T?> Find(Guid id, CancellationToken cancellationToken);
        Task Add(T aggregate, IList<IEventState> events, CancellationToken cancellationToken = default);

        Task Update(Guid id, IList<IEventState> events, CancellationToken cancellationToken = default);

        Task Delete(Guid id, IList<IEventState> events, CancellationToken cancellationToken = default);
    }
}
