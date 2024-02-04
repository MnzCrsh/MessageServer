using MessageServer.Domain;
using Microsoft.EntityFrameworkCore;

namespace MessageServer.Infrastructure;

public class PostgresDbContext : DbContext
{
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
        
    }

    private DbSet<PetOwnerDto> PetOwners { get; set; } = null!;
}