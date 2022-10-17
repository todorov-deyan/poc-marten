using MediatR;
using PocMarten.Api.Aggregates.Weather.Events;
using PocMarten.Api.Aggregates.Weather.Model;
using PocMarten.Api.Aggregates.Weather.Repository;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.Weather.Handlers
{
    public record TemperatureChangedCommand(Guid streamId, int temperatureChanged) : IRequest;


    public class TemperatureChangedCommandHaandler : IRequestHandler<TemperatureChangedCommand>
    {
        private readonly WeatherRepository _repository;

        public TemperatureChangedCommandHaandler(WeatherRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(TemperatureChangedCommand request, CancellationToken cancellationToken)
        {
            WeatherForecast? forecast = await _repository.Find(request.streamId, cancellationToken);
            if (forecast is null)
                return Unit.Value;

            List<IEventState> events = new();

            switch (Math.Sign(request.temperatureChanged))
            {
                case -1:
                    events.Add(new TemperatureLow(request.temperatureChanged));
                    break;
                case 1:
                    events.Add(new TemperatureHigh(request.temperatureChanged));
                    break;
                default:
                    return Unit.Value;
            };

            await _repository.Update(forecast.Id, events, cancellationToken);

            return Unit.Value;
        }
    }
}
