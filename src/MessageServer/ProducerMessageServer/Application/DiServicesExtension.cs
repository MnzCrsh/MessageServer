using FluentValidation;
namespace MessageServer.Application;

public static class DiServicesExtension
{
    public static void RunServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        
        
        services.AddControllers().AddNewtonsoftJson();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddValidatorsFromAssemblyContaining<Program>();
        
        services.AddHsts(options =>
        {
            const int maxTime = 1;
            options.MaxAge = TimeSpan.FromHours(maxTime);
        });
    }
}