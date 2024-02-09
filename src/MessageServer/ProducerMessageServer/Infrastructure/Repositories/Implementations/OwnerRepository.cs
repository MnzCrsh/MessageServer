using MessageServer.Domain;
using MessageServer.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MessageServer.Infrastructure.Repositories.Implementations;


public class OwnerRepository : IOwnerRepository
{
    public OwnerRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    private readonly PostgresDbContext _dbContext;
    
    public async Task<Guid> CreateAsync(Owner owner)
    {
        Owner newOwner = new Owner
        {
            Id = new Guid(),
            Name = owner.Name,
            IsMarkedToDelete = false
        };
        _dbContext.Add(newOwner);
        await _dbContext.SaveChangesAsync();
        return newOwner.Id;
    }

    public async Task<Owner> GetAsync(int id)
    {
        var existingOwner = await _dbContext.PetOwners
            .Where(p => p.Id.Equals(id) && !p.IsMarkedToDelete)
            .Select(r => r)
            .SingleOrDefaultAsync() ?? 
                            throw new InvalidOperationException($"{id} dont exist");
        return existingOwner;
    }

    public Task<IEnumerable<OwnerDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Owner owner)
    {
        throw new NotImplementedException();
    }
    

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}