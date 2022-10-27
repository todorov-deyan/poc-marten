using Newtonsoft.Json;
using PocMarten.Api.Aggregates.BicoinExchangeRate.Events;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BicoinExchangeRate.Models
{
    public class ExchangeRateDetails : Aggregate
    {
        public decimal ExchangeRate { get; private set; }

        public DateTime Date { get; private set; }

        public ExchangePosition Position { get; private set; }

        [JsonConstructor]
        private ExchangeRateDetails()
        {
            Id = Guid.NewGuid();
        }
        public ExchangeRateDetails(ExchangeRateYesterday @event)
        {
            Id = Guid.NewGuid();
            ExchangeRate = @event.ExchangeRate;
            Position = ExchangePosition.Init;
        }

        public void Apply(ExchangeRateHigh @event)
        {
            ExchangeRate = @event.ExchangeRateUsdHigh;
            Position = ExchangePosition.High;
        }

        public void Aplly(ExchangeRateLow @event)
        {
            ExchangeRate = @event.ExchangeRateUsdLow;
            Position = ExchangePosition.Low;
        }
    }
}
