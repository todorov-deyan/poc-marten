using Marten;

using Newtonsoft.Json;

using Shouldly;

using Xunit;
using Xunit.Abstractions;

namespace PocMarten.Tests
{
    public class OrderTests
    {
        private static readonly Random Random = new();

        private readonly ITestOutputHelper _output;
        private readonly DocumentStore _store;

        public OrderTests(ITestOutputHelper output)
        {
            _output = output;
            _store = DocumentStore.For("Username=postgres;Password=secretp@ssword;Host=127.0.0.1;Port=5433;Database=postgres;Pooling=true;");
        }

        [Fact]
        public async Task Clean()
        {
            await _store.Advanced.Clean.CompletelyRemoveAllAsync();
        }

        [Fact]
        public async Task Add_Order()
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid().ToString(),
                Details = new List<OrderDetail>()
                {
                    new OrderDetail
                    {
                        Number = Random.Next(1, 1000),
                        PartNumber = RandomString(5)
                    },
                    new OrderDetail
                    {
                        Number = Random.Next(1, 1000),
                        PartNumber = RandomString(5)
                    }
                },
                Priority = (Priority)Random.Next(1, 3),
                BillingAddress = new Address()
                {
                    Address1 = RandomString(10),
                    Address2 = RandomString(10),
                    City = RandomString(10),
                    Country = RandomString(10),
                    PostalCode = RandomString(10),
                    Primary = Random.Next(0, 2) == 1,
                    StateOrProvince = RandomString(9)
                }
            };

            await using var session = _store.LightweightSession();

            session.Store(order);
            await session.SaveChangesAsync();

            await using var session2 = _store.LightweightSession();
            var order2 = await session2.LoadAsync<Order>(order.Id);

            _output.WriteLine(JsonConvert.SerializeObject(order2, Formatting.Indented));

            order2.ShouldNotBeNull();
            order2.ShouldNotBeSameAs(order);
            order2.CustomerId.ShouldBe(order.CustomerId);
        }

        public enum Priority
        {
            Low,
            Medium,
            High,
        }

        public class Order
        {
            public Guid Id { get; set; }

            public Priority Priority { get; set; }

            public string CustomerId { get; set; }

            public IList<OrderDetail> Details { get; set; }

            public Address BillingAddress { get; set; }
        }

        public class OrderDetail
        {
            public string PartNumber { get; set; }

            public int Number { get; set; }
        }

        public class Address
        {
            public string Address1 { get; set; }

            public string Address2 { get; set; }

            public string City { get; set; }

            public string StateOrProvince { get; set; }

            public string Country { get; set; }

            public string PostalCode { get; set; }

            public bool Primary { get; set; }
        }

        private static string RandomString(int maxLength)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, maxLength).Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}