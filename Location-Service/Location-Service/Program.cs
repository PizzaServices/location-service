using Location_Service.Extensions;
using Location_Service.Middleware;

namespace Location_Service;

public static class Program
{
    private const string JWT_ARGUMENT = "--jwt";
    
    public static async Task Main(string[] args)
    {
        bool isJwtEnabled = args.Contains(JWT_ARGUMENT);
        
        var builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.
        builder.Services.AddServices();
        builder.Services.AddJwtServices();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseMiddleware<RequestTimeLoggingMiddleware>();
        }

        app.UseMiddleware<JwtMiddleware>();
        
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}