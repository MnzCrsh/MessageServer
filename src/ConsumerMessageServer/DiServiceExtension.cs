namespace ConsumerMessageServer;

public static class DiServiceExtension
{
    public static void RunServiceConfiguration(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers();
        
        services.AddHsts(options =>
        {
            const int maxTime = 1;
            options.MaxAge = TimeSpan.FromHours(maxTime);
        });
    }
}