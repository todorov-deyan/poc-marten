namespace PocMarten.Api.Repository
{
    public class Aggregate : IAggregate
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}
