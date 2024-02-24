using MessageServer.Domain;
using Microsoft.EntityFrameworkCore;

namespace MessageServer.Infrastructure.Repositories;

public class PostgresDbContext : DbContext
{
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
        
    }

    public DbSet<Owner> PetOwners { get; set; }
    public DbSet<Pet> Pets { get; set; }
}