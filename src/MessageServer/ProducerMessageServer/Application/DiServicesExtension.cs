using FluentValidation;

namespace MessageServer.Application;

public static class DiServicesExtension
{
    public static void RunServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddControllers().AddNewtonsoftJson();
        
        services.AddHsts(options =>
        {
            const int maxTime = 1;
            options.MaxAge = TimeSpan.FromHours(maxTime);
        });
    }
}