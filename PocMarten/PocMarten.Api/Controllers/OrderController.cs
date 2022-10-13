using Microsoft.AspNetCore.Mvc;

using PocMarten.Api.Aggregates.Order.Events;
using PocMarten.Api.Aggregates.Order.Models;
using PocMarten.Api.Aggregates.Order.Repository;
using PocMarten.Api.Common.EventSourcing;

namespace PocMarten.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private const string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random Random = new();

        private readonly OrderRepository _repository;
        private readonly ILogger<WeatherForecastController> _logger;

        public OrderController(
            OrderRepository repository,
            ILogger<WeatherForecastController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{streamId:guid}", Name = "Orders")]
        public async Task<ActionResult<OrderModel>> Get(Guid streamId, CancellationToken cancellationToken = default)
        {
            var result = await _repository.Find(streamId, cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost(Name = "CreateOrders")]
        public async Task<ActionResult<OrderModel>> Post()
        {
            var order = new OrderModel
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid().ToString(),
                Details = new List<OrderDetail>()
                {
                    new OrderDetail
                    {
                        Number = Random.Next(1, 1000),
                        PartNumber = new string(Enumerable.Repeat(Chars, 5).Select(s => s[Random.Next(s.Length)]).ToArray())
                    },
                    new OrderDetail
                    {
                        Number = Random.Next(1, 1000),
                        PartNumber = new string(Enumerable.Repeat(Chars, 5).Select(s => s[Random.Next(s.Length)]).ToArray())
                    }
                },
                Priority = (Priority)Random.Next(1, 3),
            };

            var events = new List<IEventState>
            {
                new OrderPriorityLow(Priority.Low),
                new OrderPriorityHigh(Priority.High)
            };

            await _repository.Add(order, events);

            return CreatedAtAction("Get", new { streamId = order.Id }, new { streamId = order.Id });
        }
    }
}
