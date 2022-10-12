using Marten;

namespace PocMarten.Api.Repository
{
    public class MartenRepository<T> : IMartenRepository<T> where T : class, IAggregate
    {
        private readonly IDocumentSession _documentSession;

        public MartenRepository(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }


        public Task Add(T aggregate, object[] events, CancellationToken cancellationToken = default)
        {

            _documentSession.Events.StartStream<T>(
                aggregate.Id,
                events
            );

            return _documentSession.SaveChangesAsync(cancellationToken);

        }

        public Task Delete(Guid id, object[] events, CancellationToken cancellationToken = default)
        {
            return Update(id, events , cancellationToken);
        }

        public Task<T?> Find(Guid id, CancellationToken cancellationToken = default)
        {
            return _documentSession.Events.AggregateStreamAsync<T>(id, token: cancellationToken);
        }

        public Task Update(Guid id, object[] events, CancellationToken cancellationToken = default)
        {
            _documentSession.Events.Append(id, events);

            return  _documentSession.SaveChangesAsync(cancellationToken);
        }
    }
}
