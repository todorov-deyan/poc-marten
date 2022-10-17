using MediatR;
using PocMarten.Api.Aggregates.Weather.Model;
using PocMarten.Api.Aggregates.Weather.Repository;

namespace PocMarten.Api.Aggregates.Weather.Queries
{
    public record GetWeatherByIdQuery(Guid Id) : IRequest<WeatherForecast>;

    public class GetWeatherByIdQueryHandler : IRequestHandler<GetWeatherByIdQuery, WeatherForecast>
    {
        private readonly WeatherRepository _repository;

        public GetWeatherByIdQueryHandler(WeatherRepository repository)
        {
            _repository = repository;
        }

        public async Task<WeatherForecast> Handle(GetWeatherByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.Find(request.Id, cancellationToken);

            return result;
        }
    }
}
