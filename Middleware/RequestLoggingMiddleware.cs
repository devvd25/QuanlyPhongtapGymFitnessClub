using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace QuanlyPhongtapGymFitnessClub.Middleware
{
    /// <summary>
    /// Middleware ghi log console cho má»i HTTP Request (BĂ i táº­p Buá»•i 2 - Slide 28)
    /// CĂº phĂ¡p log: [LOG] Request: {Method} {Path}
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // In ra console theo Ä‘Ăºng Ä‘á»‹nh dáº¡ng slide bĂ i táº­p: [LOG] Request: {Method} {Path}
            string logMessage = $"[LOG] Request: {context.Request.Method} {context.Request.Path}";
            Console.WriteLine(logMessage);
            _logger.LogInformation(logMessage);

            await _next(context);
        }
    }
}
