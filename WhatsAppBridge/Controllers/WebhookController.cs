using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WhatsAppBridge.Filters;

namespace WhatsAppBridge.Controllers {

    [Route("api/v1/wa/webhook")]
    [ApiController]
    public class WebhookController : ControllerBase {

        private readonly ILogger<WebhookController> _logger;
        private readonly BridgesSettings _options;

        public WebhookController(ILogger<WebhookController> logger, IOptions<BridgesSettings> iOptions) {
            _logger = logger;
            _options = iOptions.Value;
        }

        [HttpGet]
        [Route("")]
        [Route("{*path}")]
        public IActionResult VerifyWebhook([FromQuery(Name = "hub.mode")] string mode,
                                   [FromQuery(Name = "hub.verify_token")] string token,
                                   [FromQuery(Name = "hub.challenge")] string challenge) {
            if (mode == "subscribe" /*&& hub_verify_token == Environment.GetEnvironmentVariable("VERIFY_TOKEN")*/) //bizim icin islevi hic yok. sifir!
                return Content(challenge);
            return Forbid();
        }

        [HttpPost]
        [Route("")]
        [Route("{*path}")]
        [ServiceFilter(typeof(LogExecutionFilter))]
        public async Task<IActionResult> WebhookAsync(string path) {

            if (!Request.Headers.TryGetValue("x-hub-signature-256", out var signature) || string.IsNullOrEmpty(signature)) {
                _logger.LogWarning("x-hub-signature-256 header is missing or empty.");
                return Ok(); //return immediately.
            }

            if (Request.Body == null) {
                _logger.LogWarning("Request.Body is is missing or empty.");
                return Ok(); //return immediately.
            }

            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync(); //use Async for thread safety.

            return Ok(); //return immediately. It is necessary.
        }
    }
}
