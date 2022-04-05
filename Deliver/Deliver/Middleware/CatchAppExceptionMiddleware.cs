using Models.Exceptions;
using Models.Response._Core;
using Newtonsoft.Json;

namespace Deliver.Middleware
{
    public class CatchAppExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CatchAppExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IHostEnvironment environment)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                context.Response.StatusCode = 200;
                await context.Response
                    .WriteAsync(JsonConvert.SerializeObject(BaseRespons.Fail(ex.Message)));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 200;
                if (environment.IsProduction())
                {
                    await context.Response
                        .WriteAsync(JsonConvert.SerializeObject(BaseRespons.Fail("Something went wrong.")));
                }
                else
                {
                    await context.Response
                        .WriteAsync(JsonConvert.SerializeObject(BaseRespons.Fail(ex.Message)));
                }
            }
        }
    }
}
