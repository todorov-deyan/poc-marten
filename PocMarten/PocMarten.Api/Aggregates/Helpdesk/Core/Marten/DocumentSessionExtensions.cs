using Marten;

namespace PocMarten.Api.Aggregates.Helpdesk.Core.Marten
{
    public static class DocumentSessionExtensions
    {
        public static Task Add<T>(this IDocumentSession documentSession, Guid id, object @event, CancellationToken cancellation)
            where T : class
        {
            documentSession.Events.StartStream<T>(id, @event);
            return documentSession.SaveChangesAsync(cancellation);
        }

        public static Task GetAndUpdate<T>(this IDocumentSession documentSession, Guid id, int version, Func<T, object> handle, CancellationToken cancellation) 
            where T : class
        {
            return documentSession.Events.WriteToAggregate<T>(id, version, stream => stream.AppendOne(handle(stream.Aggregate)), cancellation);
        }
    }
}
