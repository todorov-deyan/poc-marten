using Marten;

using PocMarten.Api.Aggregates.Weather.Model;
using PocMarten.Api.Common.Repository;

namespace PocMarten.Api.Aggregates.Weather.Repository
{
    public class WeatherRepository : MartenRepository<WeatherForecast>
    {
        public WeatherRepository(IDocumentSession session)
            : base(session)
        {
        }
    }
}
