using Ardalis.Result;
using MediatR;
using PocMarten.Api.Aggregates.Weather.Events;
using PocMarten.Api.Aggregates.Weather.Model;
using PocMarten.Api.Aggregates.Weather.Repository;
using PocMarten.Api.Common.EventSourcing;
using PocMarten.Api.Common.CQRS;

namespace PocMarten.Api.Aggregates.Weather.Commands
{
    public record CurrentTemperatureCommand(int currentTemperature) : ICommandRequest<Result<WeatherForecast>>;

    public class CurrentTemperatureHandler : ICommandHandler<CurrentTemperatureCommand, Result<WeatherForecast>>
    {
        private readonly WeatherRepository _repository;

        public CurrentTemperatureHandler(WeatherRepository repository)
        {
            _repository = repository;
        }


        public async Task<Result<WeatherForecast>> Handle(CurrentTemperatureCommand request, CancellationToken cancellationToken)
        {
            var initWeather = new TemperatureMonitoringStarted(request.currentTemperature);
            
            WeatherForecast weatherForecast = new WeatherForecast(initWeather);
            
            List<IEventState> events = new();

            events.Add(initWeather);

            await _repository.Add(weatherForecast, events, cancellationToken);

            return  Result<WeatherForecast>.Success(weatherForecast);
        }
    }
}
