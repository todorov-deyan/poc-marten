using Marten;
using Newtonsoft.Json;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace PocMarten.Tests
{
    public class InvoiceTests
    {
        private readonly ITestOutputHelper _output;
        private readonly DocumentStore _store;

        public InvoiceTests(ITestOutputHelper output)
        {
            _output = output;
            _store = DocumentStore.For("Username=postgres;Password=secretp@ssword;Host=127.0.0.1;Port=5432;Database=postgres;Pooling=true;");
        }

        [Fact]
        public async Task Clean()
        {
            await _store.Advanced.Clean.CompletelyRemoveAllAsync();
        }

        [Fact]
        public async Task Add_Invoice()
        {
            var firstInvoice = new Invoice
            {
                Id = Guid.NewGuid(),
                Amount = 50,
                DateIssued = DateTimeOffset.UtcNow,
                Status = AmountType.NetoAmount
            };

            await using var sessionOne = _store.LightweightSession();

            sessionOne.Store(firstInvoice);
            await sessionOne.SaveChangesAsync();

            await using var sessionTwo = _store.LightweightSession();
            var secondInvoice = await sessionTwo.LoadAsync<Invoice>(firstInvoice.Id);

            _output.WriteLine(JsonConvert.SerializeObject(secondInvoice, Formatting.Indented));

            secondInvoice.ShouldNotBeNull();
            secondInvoice.Equals(firstInvoice);
            secondInvoice.Amount.ShouldBe(firstInvoice.Amount);
        }

        public class Invoice
        {
            public Guid Id { get; set; }
            public decimal Amount { get; set; }
            public AmountType Status { get;  set; }

            public DateTimeOffset DateIssued { get;  set; }
        }

        public enum AmountType
        {
            NetoAmount = 0,
            GrossAmount = 1,
        }
    }
}
