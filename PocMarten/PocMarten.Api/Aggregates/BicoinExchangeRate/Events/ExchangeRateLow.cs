using PocMarten.Api.Common.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocMarten.Api.Aggregates.BicoinExchangeRate.Events
{
    public class ExchangeRateLow : IEventState
    {
        public decimal ExchangeRateUsdLow { get; set; }

        public ExchangeRateLow(decimal usd)
        {
            ExchangeRateUsdLow = usd;
        }
    }
}
