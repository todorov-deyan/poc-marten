using Marten;
using PocMarten.Api.Repository;

namespace PocMarten.Api.Aggregates.Weather.Respository
{
    public class WeatherRepository : MartenRepository<WeatherForecast>
    {
        public WeatherRepository(IDocumentSession session) : base(session)
        {

        }
    }
}
