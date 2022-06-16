﻿using Models.Exceptions;
using Models.Logger;
using Models.Response._Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                Formatting = Formatting.Indented,
            };

            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                context.Response.StatusCode = 400;

                await context.Response
                    .WriteAsync(JsonConvert.SerializeObject(BaseRespons.Fail(ex.Message), jsonSerializerSettings));
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
                        .WriteAsync(JsonConvert.SerializeObject(BaseRespons.Fail("Something went wrong."), jsonSerializerSettings));
                }
                else
                {
                    await context.Response
                        .WriteAsync(JsonConvert.SerializeObject(BaseRespons.Fail(ex.Message), jsonSerializerSettings));
                }
            }
        }
    }
}
