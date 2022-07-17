using Deliver.Settings;
using Models.Exceptions;
using Models.Logger;
using Models.Response._Core;
using Newtonsoft.Json;
using Services.Interface;

namespace Deliver.Middleware
{
    public class CatchAppExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CatchAppExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IHostEnvironment environment, ILogService logger)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                context.Response.StatusCode = ex.StatusCode ?? StatusCodes.Status400BadRequest;

                await context.Response
                    .WriteAsync(JsonConvert.SerializeObject(BaseRespons.Fail(ex.Message), JsonSettings.GetJsonSerializerSettings()));
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage
                {
                    Message = ex.Message,
                    InnerException = ex.InnerException?.Message,
                    StackTrace = ex.StackTrace,
                };

                await logger.LogError(logMessage);
                context.Response.StatusCode = 500;
                if (environment.IsProduction())
                {
                    await context.Response
                        .WriteAsync(JsonConvert.SerializeObject(BaseRespons.Fail("Something went wrong."), JsonSettings.GetJsonSerializerSettings()));
                }
                else
                {
                    await context.Response
                        .WriteAsync(JsonConvert.SerializeObject(BaseRespons.Fail(ex.Message), JsonSettings.GetJsonSerializerSettings()));
                }
            }
        }

    }
}
