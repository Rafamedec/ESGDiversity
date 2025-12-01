using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ESGDiversity.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = "An error occurred while processing your request",
                Detail = exception.Message,
                Instance = context.Request.Path
            };

            if (exception is ArgumentException || exception is InvalidOperationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                problemDetails.Status = (int)statusCode;
                problemDetails.Title = "Bad Request";
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(problemDetails);
            return context.Response.WriteAsync(json);
        }
    }
}
