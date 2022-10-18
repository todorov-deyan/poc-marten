using Ardalis.Result;
using MediatR;
using PocMarten.Api.Aggregates.Weather.Events;
using PocMarten.Api.Aggregates.Weather.Model;
using PocMarten.Api.Aggregates.Weather.Repository;
using PocMarten.Api.Common.EventSourcing;
using PocMarten.Api.Common.CQRS;

namespace PocMarten.Api.Aggregates.Weather.Commands
{
    public record TemperatureChangedCommand(Guid streamId, int temperatureChanged) : ICommandRequest<Result>;


    public class TemperatureChangedCommandHaandler : ICommandHandler<TemperatureChangedCommand, Result>
    {
        private readonly WeatherRepository _repository;

        public TemperatureChangedCommandHaandler(WeatherRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(TemperatureChangedCommand request, CancellationToken cancellationToken)
        {
            WeatherForecast? forecast = await _repository.Find(request.streamId, cancellationToken);
            if (forecast is null)
                return Result.NotFound();

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
                    return Result.Error("Bad Request");
            };

            await _repository.Update(forecast.Id, events, cancellationToken);

            return Result.Success();
        }
    }
}
