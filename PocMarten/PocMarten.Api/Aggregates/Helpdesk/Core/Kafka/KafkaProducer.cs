using System.Text.Json;

using Confluent.Kafka;

using Marten;
using Marten.Events;
using Marten.Events.Projections;


namespace PocMarten.Api.Aggregates.Helpdesk.Core.Kafka
{
    public class KafkaProducer : IProjection
    {
        private readonly KafkaProducerConfig _configuration;

        private const string DefaultConfigKey = "";

        public KafkaProducer(IConfiguration configuration)
        {
            _configuration = configuration.GetSection(DefaultConfigKey).Get<KafkaProducerConfig>();
        }

        public void Apply(IDocumentOperations operations, IReadOnlyList<StreamAction> streams)
            => throw new NotImplementedException($"{nameof(KafkaProducer)} should be only used in the AsyncDeamon.");

        public async Task ApplyAsync(IDocumentOperations operations, IReadOnlyList<StreamAction> streams, CancellationToken cancellation)
        {
            foreach (var @event in streams.SelectMany(streamAction => streamAction.Events))
            {
                await Publish(@event.Data, cancellation);
            }
        }

        private async Task Publish(object @event, CancellationToken cancellation)
        {
            try
            {
                using var producer = new ProducerBuilder<string, string>(_configuration.ProducerConfig).Build();

                await producer
                    .ProduceAsync(_configuration.Topic,
                        new Message<string, string>
                        {
                            // store event type name in message key
                            Key = @event.GetType().Name,
                            // serialize event to message value
                            Value = JsonSerializer.Serialize(@event),
                        }, cancellation)
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
