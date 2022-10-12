namespace PocMarten.Api.Common.EventSourcing
{
    public class Aggregate : IAggregate
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}
