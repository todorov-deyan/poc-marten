using Marten;
using Newtonsoft.Json;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace PocMarten.Tests
{
    public class ExchangeRateTest
    {
        private readonly ITestOutputHelper _output;
        private DocumentStore _store;

        public ExchangeRateTest(ITestOutputHelper output)
        {
            _output = output;
            _store = DocumentStore.For("Username=postgres;Password=secretp@ssword;Host=127.0.0.1;Port=5432;Database=postgres;Pooling=true;");
        }

        [Fact]
        public async Task Clean()
        {
            await _store.Advanced.Clean.CompletelyRemoveAllAsync();
        }

        [Fact] async Task Add_ExchangeRate()
        {
            var exchangeRate = new ExchangeRate
            {
                Id = Guid.NewGuid(),
                ExchangeRateAmount = 18999,
                Position = ExchangePosition.Low
            };


            await using var sessionOne = _store.LightweightSession();

            sessionOne.Store(exchangeRate);
            await sessionOne.SaveChangesAsync();

            await using var sessionTwo = _store.LightweightSession();
            var exchangeRateNew = await sessionTwo.LoadAsync<ExchangeRate>(exchangeRate.Id);

            _output.WriteLine(JsonConvert.SerializeObject(exchangeRateNew, Formatting.Indented));

            exchangeRateNew.ShouldNotBeNull();
            exchangeRateNew.Equals(exchangeRate);
            exchangeRateNew.ExchangeRateAmount.ShouldBe(exchangeRate.ExchangeRateAmount);
        }

        public class ExchangeRate
        {
            public Guid Id { get; set; }

            public decimal ExchangeRateAmount { get; set; }

            public ExchangePosition Position { get; set; }
        }

        public enum ExchangePosition
        {
            None = 0,
            Low = 1,
            High = 2,
            Init = 3
        }
    }
}
