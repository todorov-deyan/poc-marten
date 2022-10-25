using Confluent.Kafka;

namespace PocMarten.Api.Aggregates.Helpdesk.Core.Kafka
{
    public class KafkaProducerConfig
    {
        public ProducerConfig? ProducerConfig { get; set; }

        public string? Topic { get; set; }
    }
}
