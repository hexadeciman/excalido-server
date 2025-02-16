using System.Text.Json;

namespace Api.Middleware;

public class ResponseWrapperMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using var newBodyStream = new MemoryStream();
        context.Response.Body = newBodyStream;

        await next(context);

        context.Response.Body = originalBodyStream;
        newBodyStream.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(newBodyStream).ReadToEndAsync();

        var wrappedResponse = new
        {
            status = context.Response.StatusCode,
            data = string.IsNullOrWhiteSpace(body) ? null : JsonSerializer.Deserialize<object>(body)
        };

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(wrappedResponse));
    }
}