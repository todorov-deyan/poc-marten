namespace PocMarten.Api.Repository
{
    public interface IMartenRepository<T> where T : class, IAggregate
    {
        Task<T?> Find(Guid id, CancellationToken cancellationToken);
        Task Add(T aggregate, object[] events, CancellationToken cancellationToken = default);

        Task Update(Guid id, object[] events, CancellationToken cancellationToken = default);

        Task Delete(Guid id, object[] events, CancellationToken cancellationToken = default);
    }
}
