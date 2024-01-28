namespace MessageServer;

public static class DiServicesExtension
{
    public static void RunServices(this IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddHsts(options =>
        {
            const int maxTime = 1;
            options.MaxAge = TimeSpan.FromHours(maxTime);
        });
    }
}