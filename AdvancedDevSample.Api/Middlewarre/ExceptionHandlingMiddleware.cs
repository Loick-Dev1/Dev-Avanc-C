using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Infrastructure.Exceptions;
using System.Net;
using System.Text.Json;

namespace AdvancedDevSample.Api.Middlewarre
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
            catch (DomainException ex)
            {
                _logger.LogError(ex, "Erreur Métier");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(
                    new
                    {
                        title = "Erreur Métier",
                        detail = ex.Message
                    });
            }
            catch (ApplicationServiceException ex)
            {
                _logger.LogWarning(ex, "Erreur application");

                context.Response.StatusCode = (int)ex.StatusCode;
                await context.Response.WriteAsJsonAsync(
                    new { title = "Ressource introuvable", detail = ex.Message });
            }

            catch (InfrastructureException ex)
            {
                _logger.LogError(ex, "Erreur technique");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(new { error = "Erreur technique" }));
            }

            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Erreur inattendue");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
            }


        }
    }

}
