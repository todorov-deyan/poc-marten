using MediatR;
using PocMarten.Api.Aggregates.Weather.Events;
using PocMarten.Api.Aggregates.Weather.Model;
using PocMarten.Api.Aggregates.Weather.Repository;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Weather.Handlers
{
    public record CurrentTemperatureCommand(int currentTemperature) : IRequest<Guid>;

    public class CurrentTemperatureHandler : IRequestHandler<CurrentTemperatureCommand, Guid>
    {
        private readonly WeatherRepository _repository;

        public CurrentTemperatureHandler(WeatherRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CurrentTemperatureCommand request, CancellationToken cancellationToken)
        {
            var initWeather = new TemperatureMonitoringStarted(request.currentTemperature);


            WeatherForecast weatherForecast = new WeatherForecast(initWeather);
            weatherForecast.Id = Guid.NewGuid();
            //weatherForecast.TemperatureC = initWeather.TemperatureC;

            List<IEventState> events = new();

            events.Add(initWeather);

            await _repository.Add(weatherForecast, events, cancellationToken);

            return weatherForecast.Id;
        }
    }
}
