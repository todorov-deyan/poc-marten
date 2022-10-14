using PocMarten.Api.Common.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocMarten.Api.Aggregates.BicoinExchangeRate.Events
{
    public class ExchangeRateHigh : IEventState
    {
        public decimal ExchangeRateUsdHigh { get; set; }

        public ExchangeRateHigh(decimal usd)
        {
            ExchangeRateUsdHigh = usd;
        }
    }
}
