using MessageServer.Domain;
using Microsoft.EntityFrameworkCore;

namespace MessageServer.Infrastructure.Repositories;

public class PostgresDbContext : DbContext
{
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
        
    }

    public virtual DbSet<Owner> PetOwners { get; set; } = null!;
}