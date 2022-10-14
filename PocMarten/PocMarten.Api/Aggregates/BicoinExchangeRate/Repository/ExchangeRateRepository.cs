using Marten;
using PocMarten.Api.Aggregates.BicoinExchangeRate.Models;
using PocMarten.Api.Common.Repository;

namespace PocMarten.Api.Aggregates.BicoinExchangeRate.Repository
{
    public class ExchangeRateRepository : MartenRepository<ExchangeRateDetails>
    {
        public ExchangeRateRepository(IDocumentSession documentSession) 
            : base(documentSession)
        {
        }
    }
}
