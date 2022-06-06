using System.Diagnostics;

namespace Location_Service.Middleware;

public class RequestTimeLoggingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        await next(context);
        
        stopwatch.Stop();
        Console.WriteLine($"Request took about {stopwatch.ElapsedMilliseconds}ms.");
    }
}