namespace PocMarten.Api.Common.EventSourcing
{
    public abstract class Aggregate : IAggregate
    {
        public Guid Id { get; init; }

        public int Version { get; }

    }
}
