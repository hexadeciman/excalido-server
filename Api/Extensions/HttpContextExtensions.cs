namespace Api.Extensions;

public static class HttpContextExtensions
{
    public static string? GetUsername(this HttpContext httpContext)
    {
        return httpContext.User.Identity?.Name;
    }

    public static Task<IResult> GetUsernameOrUnauthorized(this HttpContext httpContext, Func<string, Task<IResult>> action)
    {
        var username = httpContext.GetUsername();
        if (string.IsNullOrEmpty(username))
            return Task.FromResult(TypedResults.Unauthorized() as IResult); // ✅ Ensure Task<IResult> is returned

        return action(username);
    }
}
