using PocMarten.Api.Aggregates.BicoinExchangeRate.Events;
using PocMarten.Api.Aggregates.BicoinExchangeRate.Models;
using PocMarten.Api.Aggregates.BicoinExchangeRate.Repository;
using PocMarten.Api.Common.CQRS;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Aggregates.BicoinExchangeRate.Commands
{
    public record CurrentExchangeRateCommand(decimal currentExchangeRate) : ICommandRequest<ExchangeRateDetails>;

    public class CurremtExhangeRateHandler : ICommandHandler<CurrentExchangeRateCommand, ExchangeRateDetails>
    {
        private static readonly Random Random = new();
        private readonly ExchangeRateRepository _repository;

        public CurremtExhangeRateHandler(ExchangeRateRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExchangeRateDetails> Handle(CurrentExchangeRateCommand request, CancellationToken cancellationToken)
        {
            var exchangeRate = new ExchangeRateYesterday(request.currentExchangeRate);
            
            ExchangeRateDetails exchangeRateDetails = new ExchangeRateDetails(exchangeRate);

            List<IEventState> events = new();

            var bitcoinLow = Random.Next(0, 1000);
            events.Add(new ExchangeRateLow(exchangeRateDetails.ExchangeRate + bitcoinLow * (-1)));

            var bitcoinHigh = Random.Next(1, 1000);
            events.Add(new ExchangeRateHigh(exchangeRateDetails.ExchangeRate + bitcoinHigh));

            await _repository.Add(exchangeRateDetails, events, cancellationToken);

            return exchangeRateDetails;
        }
    }
}
