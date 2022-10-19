using Ardalis.Result;
using MediatR;
using PocMarten.Api.Aggregates.Weather.Model;
using PocMarten.Api.Aggregates.Weather.Repository;
using PocMarten.Api.Common.CQRS;

namespace PocMarten.Api.Aggregates.Weather.Queries
{
    public record GetWeatherByIdQuery(Guid Id) : IQueryRequest<Result<WeatherForecast>>;

    public class GetWeatherByIdQueryHandler : IQueryHandler<GetWeatherByIdQuery, Result<WeatherForecast>>
    {
        private readonly WeatherRepository _repository;

        public GetWeatherByIdQueryHandler(WeatherRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<WeatherForecast>> Handle(GetWeatherByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.Find(request.Id, cancellationToken);
            if(result is null)
                return Result.NotFound();

            return Result<WeatherForecast>.Success(result);
        }
    }
}
