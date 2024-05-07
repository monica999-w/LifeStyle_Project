using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace LifeStyle.Infrastructure.Middleware
{
    public class RequestProcessingTimeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestProcessingTimeMiddleware> _logger;

        public RequestProcessingTimeMiddleware(RequestDelegate next, ILogger<RequestProcessingTimeMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var startTime = DateTimeOffset.UtcNow;

            await _next(context);

            var processingTime = DateTimeOffset.UtcNow - startTime;

           
            var requestName = context.Request.Path;

            _logger.LogInformation($"Request '{requestName}' processed in {processingTime.TotalMilliseconds} ms");
        }
    }
}