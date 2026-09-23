using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Buoi2_WebAPI.Middleware
{
    /// <summary>
    /// Middleware ghi log console cho mọi HTTP Request (Bài tập Buổi 2 - Slide 28)
    /// Cú pháp log: [LOG] Request: {Method} {Path}
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
            // In ra console theo đúng định dạng slide bài tập: [LOG] Request: {Method} {Path}
            string logMessage = $"[LOG] Request: {context.Request.Method} {context.Request.Path}";
            Console.WriteLine(logMessage);
            _logger.LogInformation(logMessage);

            await _next(context);
        }
    }
}
