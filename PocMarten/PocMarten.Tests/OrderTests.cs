using System.Diagnostics;

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
        private readonly Stopwatch _stopwatch = new Stopwatch();

        private readonly DocumentStore _store;

        public OrderTests(ITestOutputHelper output)
        {
            _output = output;
            _store = DocumentStore.For(x =>
            {
                x.Connection("Username=postgres;Password=secretp@ssword;Host=127.0.0.1;Port=5432;Database=postgres;Pooling=true;");
                x.Schema.For<Order>()
                    .Index(x=>x.CustomerId)
                    .AddSubClassHierarchy();
            });
        }

        [Fact]
        public async Task Clean()
        {
            await _store.Advanced.Clean.CompletelyRemoveAllAsync();
        }

        [Fact]
        public async Task Add_Order()
        {
            _stopwatch.Start();
            var order = GetRandomOrders(1).FirstOrDefault();
            _stopwatch.Stop();
            _output.WriteLine("Random generation: " + _stopwatch.Elapsed);


            _stopwatch.Reset();
            _stopwatch.Start();

            await using var session = _store.LightweightSession();
            session.Store(order);
            await session.SaveChangesAsync();

            await using var session2 = _store.LightweightSession();
            var order2 = await session2.LoadAsync<Order>(order.Id);
            
            _stopwatch.Stop();
            _output.WriteLine("Marten query: " + _stopwatch.Elapsed);

            _output.WriteLine(JsonConvert.SerializeObject(order2, Formatting.Indented));

            order2.ShouldNotBeNull();
            order2.ShouldNotBeSameAs(order);
            order2.CustomerId.ShouldBe(order.CustomerId);
        }

        [Fact]
        public async Task Acid_Compliant_Test_1000()
        {
            await AcidRun(1000);
        }

        [Fact]
        public async Task Acid_Compliant_Test_10000()
        {
            await AcidRun(10000);
        }

        [Fact]
        public async Task Acid_Compliant_Test_100000()
        {
            await AcidRun(100000);
        }

        [Fact]
        public async Task Acid_Compliant_Test_1000000()
        {
            await AcidRun(1000000);
        }

        [Fact]
        public async Task Acid_Compliant_Test_10000000()
        {
            await AcidRun(10000000);
        }

        [Fact]
        public async Task Acid_Compliant_Test_100000000()
        {
            await AcidRun(100000000);
        }

        [Fact]
        public async Task Query_Martin()
        {
            await using var session = _store.LightweightSession();
            var ordersA = await session
                .Query<Order>()
                .Where(x => x.CustomerId.Contains("a"))
                .ToListAsync();

            _output.WriteLine("Orders with CustomerId with 'a' are {ordersA.Count}");

            var cmd = session
                .Query<Order>()
                .Where(x => x.CustomerId.Contains("a"))
                .ToCommand();

            _output.WriteLine(cmd.CommandText);

            ordersA.Count.ShouldBeGreaterThan(0);
        }

        private async Task AcidRun(int count)
        {
            _stopwatch.Start();
            var orders = GetRandomOrders(count).ToArray();
            var greenCount = orders.Length;
            _stopwatch.Stop();
            _output.WriteLine("Random generation: " + _stopwatch.Elapsed);

            greenCount.ShouldBeGreaterThan(0);
            greenCount.ShouldBeLessThan(count + 1);

            _stopwatch.Reset();
            _stopwatch.Start();

            await using var session = _store.LightweightSession();
            session.Store(orders);
            await session.SaveChangesAsync();

            await using var session2 = _store.LightweightSession();
            var dbCount = await session2
                .Query<Order>()
                .CountAsync();

            _stopwatch.Stop();
            _output.WriteLine("Marten query: " + _stopwatch.Elapsed);

            dbCount.ShouldBeGreaterThan(count - 1);
        }

        private IEnumerable<Order> GetRandomOrders(int count)
        {
            var orders = new List<Order>();

            for (var i = 0; i < count; i++)
            {
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    CustomerId = Guid.NewGuid().ToString(),
                    Details = new List<OrderDetail>(),
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

                for (var j = 0; j < Random.Next(1,6); j++)
                {
                    var detail = new OrderDetail
                    {
                        Number = Random.Next(1, 1000),
                        PartNumber = RandomString(5)
                    };

                    order.Details.Add(detail);
                }

                orders.Add(order);
            }

            return orders;
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