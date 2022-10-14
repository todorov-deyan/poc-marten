using PocMarten.Api.Common.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
