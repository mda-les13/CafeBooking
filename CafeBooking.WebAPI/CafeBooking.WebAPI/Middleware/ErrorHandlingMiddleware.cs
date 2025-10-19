using FluentValidation;
using System.Text.Json;

namespace CafeBooking.WebAPI.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = 500;
            var message = "Произошла внутренняя ошибка сервера";
            var errors = new List<Dictionary<string, string>>();

            // Обработка специфических исключений
            if (exception is ValidationException validationException)
            {
                statusCode = 400;
                message = "Ошибка валидации";
                errors = validationException.Errors
                    .Select(e => new Dictionary<string, string>
                    {
                    { "propertyName", e.PropertyName },
                    { "errorMessage", e.ErrorMessage }
                    })
                    .ToList();
            }
            else if (exception is InvalidOperationException)
            {
                statusCode = 400;
                message = exception.Message;
            }
            else
            {
                errors.Add(new Dictionary<string, string> { { "message", exception.Message } });
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                status = statusCode,
                message,
                errors
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }
    }
}
