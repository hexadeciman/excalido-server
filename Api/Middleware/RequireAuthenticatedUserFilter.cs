namespace Api.Middleware;

public class RequireAuthenticatedUserFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var httpContext = context.HttpContext;
        var username = httpContext.User.Identity?.Name;

        if (string.IsNullOrEmpty(username))
        {
            return TypedResults.Unauthorized(); // ✅ Return Unauthorized if username is missing
        }

        // ✅ Pass the username to the next endpoint handler
        httpContext.Items["Username"] = username;

        return await next(context);
    }
}
