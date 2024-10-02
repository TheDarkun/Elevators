namespace Elevators.Authentication;

public class LogoutMiddleware(RequestDelegate next, ILogger<LogoutMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            if (context.Request.Path.Equals("/logout", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.Cookies.Delete("account");
                context.Response.Redirect("/");
                return; // there is no need for invoking _next since there is a redirect
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Something went wrong during logout.");
        }

        await next(context);
    }
}
