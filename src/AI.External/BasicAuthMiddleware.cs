namespace AI.External;

public class BasicAuthMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IServiceProvider provider, IConfiguration configuration)
    {
        var token = context.Request.Headers.Authorization.ToString();
        if (!token.StartsWith("Bearer"))
        {
            context.Response.StatusCode = 401;
            return;
        }
        var tokenValue = token["Bearer ".Length..];
        var expectedToken = configuration.GetSection("Auth:Token").Value;
        if (tokenValue != expectedToken)
        {
            context.Response.StatusCode = 401;
            return;
        }
        Console.WriteLine("Auth OK");
        await next(context);
    }
}

public static class BasicAuthMiddlewareExtensions
{
    public static IApplicationBuilder UseBasicAuth(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BasicAuthMiddleware>();
    }
}