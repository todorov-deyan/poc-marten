using PocMarten.Api.Aggregates.BicoinExchangeRate.Events;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BicoinExchangeRate.Models
{
    public class ExchangeRateDetails : Aggregate
    {
        public decimal ExchangeRate { get; set; }

        public DateTime Date { get; set; }

        public ExchangePosition Position { get; set; }

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
