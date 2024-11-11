using ILogger = Serilog.ILogger;

namespace AI.External;

public class BasicAuthMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IServiceProvider provider, IConfiguration configuration, ILogger logger)
    {
        var token = context.Request.Headers.Authorization.ToString();
        if (!token.StartsWith("Bearer"))
        {
            logger.Error("No auth token provided");
            context.Response.StatusCode = 401;
            return;
        }
        var tokenValue = token["Bearer ".Length..];
        var expectedToken = configuration.GetSection("Auth:Token").Value;
        if (tokenValue != expectedToken)
        {
            logger.Error("Invalid auth token provided (unauthorized)");
            context.Response.StatusCode = 401;
            return;
        }
        logger.Information("Auth token OK");
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