using MessageServer.Infrastructure;
using MessageServer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MessageServer.Application;

public static class DbContextServiceExtension
{
    public static void RunPostgresDb(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration.GetConnectionString("pgString") ??
                         throw new ArgumentNullException(
                             "builder.Configuration.GetConnectionString(\"pgString\")");

        builder.Services.AddDbContext<PostgresDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(connString);
        } );
    }
}