using Marten;
using PocMarten.Api.Aggregates.Weather;

namespace PocMarten.Api.Repository
{
    public class WeatherRepository : MartenRepository<WeatherForecast>
    {
        public WeatherRepository(IDocumentSession session) : base(session)
        {
            
        }
    }
}
