using MessageServer.Domain;
using MessageServer.Infrastructure.Repositories.Abstractions;

namespace MessageServer.Infrastructure.Repositories.Implementations;

public class OwnerEventStrategy : IOwnerEventStrategy
{
    private readonly PostgresDbContext _dbContext;

    public OwnerEventStrategy(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandlePetAdded(Owner owner)
    {
        _dbContext.Update(owner);
        await _dbContext.SaveChangesAsync();
    }

    public async Task HandlePetRemoved(Owner owner)
    {
        _dbContext.Update(owner);
        await _dbContext.SaveChangesAsync();
    }
}