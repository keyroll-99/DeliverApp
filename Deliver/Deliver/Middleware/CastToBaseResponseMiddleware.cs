using Deliver.Settings;
using Models.Response._Core;
using Newtonsoft.Json;
using Services.Interface;
using System.Text;

namespace Deliver.Middleware;

public class CastToBaseResponseMiddleware
{
    private readonly RequestDelegate _next;

    public CastToBaseResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        Stream orginalBody = context.Response.Body;

        using var memStream = new MemoryStream();
        context.Response.Body = memStream;

        try
        {
            await _next(context);

            memStream.Position = 0;

            var responseBody = JsonConvert.DeserializeObject<object>(new StreamReader(memStream).ReadToEnd());

            if (responseBody is not BaseRespons && context.Response.StatusCode == 200)
            {
                memStream.Position = 0;
                var response = BaseRespons<object>.Success(responseBody);
                var serializecResponse = JsonConvert.SerializeObject(response, JsonSettings.GetJsonSerializerSettings());

                await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(serializecResponse));
            }
        }
        finally {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            await context.Response.Body.CopyToAsync(orginalBody);
            context.Response.Body = orginalBody;
        }
    }
}
