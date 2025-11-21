using System.Net;
using System.Text.Json;
using FluentValidation;
using ProductsApi.Exceptions;


namespace ProductsApi.Middlewares
{
   
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled exception occurred.");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            object response;
            HttpStatusCode statusCode;

            switch (exception)
            {
                case ValidationException validationEx:
                    response = new
                    {
                        message = "Validation failed",
                        errors = validationEx.Errors.Select(e => new
                        {
                            field = e.PropertyName,
                            message = e.ErrorMessage
                        }),
                        timestamp = DateTime.UtcNow
                    };
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case InvalidOperationException invalidOpEx:
                    response = new
                    {
                        message = "Invalid operation",
                        error = invalidOpEx.Message,
                        timestamp = DateTime.UtcNow
                    };
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case NotFoundException notFoundEx:
                    response = new
                    {
                        message = notFoundEx.Message,
                        timestamp = DateTime.UtcNow
                    };
                    statusCode = HttpStatusCode.NotFound;
                    break;

                default:
                    response = new
                    {
                        message = "An unexpected error occurred.",
                        detail = exception.Message,
                        timestamp = DateTime.UtcNow
                    };
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            var json = JsonSerializer.Serialize(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(json);
        }
    }

}
