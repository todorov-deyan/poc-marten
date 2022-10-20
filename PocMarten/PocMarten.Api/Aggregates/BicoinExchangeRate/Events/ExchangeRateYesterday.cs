using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BicoinExchangeRate.Events
{
    public class ExchangeRateYesterday : IEventState
    {
        public decimal ExchangeRate { get; set; }

        public ExchangeRateYesterday(decimal exchangeRate)
        {
            ExchangeRate = exchangeRate;
        }
    }
}
