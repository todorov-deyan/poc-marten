using Microsoft.AspNetCore.Mvc;

namespace PocMarten.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SlowRequestController : ControllerBase
    {
        private readonly ILogger _logger;

        public SlowRequestController(ILogger<SlowRequestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/slowtest")]
        public async Task<string> Get()
        {
            _logger.LogInformation("Starting to do slow work");

            // slow async action, e.g. call external api
            await Task.Delay(10_000);

            var message = "Finished slow delay of 10 seconds.";

            _logger.LogInformation(message);

            return message;
        }

        [HttpGet("/slowtest-with-cancellation-token")]
        public async Task<string> GetWithCancellationToken(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting to do slow work");

            // slow async action, e.g. call external api
            await Task.Delay(10_000, cancellationToken);

            var message = "Finished slow delay of 10 seconds.";

            _logger.LogInformation(message);

            return message;
        }
    }
}
