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
        var newOwner = new Owner
        {
            Id = new Guid(),
            Name = owner.Name,
            IsMarkedToDelete = false,
            PassportNumber = owner.PassportNumber,
            PassportSeries = owner.PassportSeries
        };
        _dbContext.Add(newOwner);
        await _dbContext.SaveChangesAsync();
        return newOwner.Id;
    }

    public async Task<Owner> GetAsync(int id)
    {
        var existingOwner = await _dbContext.PetOwners
            .Where(o => o.Id.Equals(id) && !o.IsMarkedToDelete)
            .Select(o => o)
            .SingleOrDefaultAsync() ?? 
                            throw new InvalidOperationException($"{id} dont exist");
        return existingOwner;
    }

    public async Task<IEnumerable<OwnerDto>> GetAllAsync()
    {
        var owners = await _dbContext.PetOwners
            .Where(o => !o.IsMarkedToDelete)
            .Select(o => new OwnerDto
            {
                Id = o.Id,
                Name = o.Name,
                OwnedPets = o.OwnedPets!
                    .Select(pet => new PetDto
                    { 
                        Id = pet.Id,
                        Name = pet.Name,
                        PetOwner = pet.PetOwner 
                    })
            }).ToListAsync();
        return owners;
    }

    public async Task UpdateAsync(Owner owner)
    {
        var oldOwnerData = await _dbContext.PetOwners
            .FirstOrDefaultAsync(o => o.Id.Equals(owner.Id));
        
        UpdateOwnerData(oldOwnerData ?? throw new ArgumentNullException
            ($"Cant find owner with ID: {owner.Id}"), owner);

        await _dbContext.SaveChangesAsync();
    }
    

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    private void UpdateOwnerData(Owner oldOwnerData, Owner newOwnerData)
    {
        oldOwnerData.Name = newOwnerData.Name;
        oldOwnerData.PassportNumber = newOwnerData.PassportNumber;
        oldOwnerData.PassportSeries = newOwnerData.PassportSeries;
        oldOwnerData.IsMarkedToDelete = newOwnerData.IsMarkedToDelete;
        
        // Get information about entity state
        var entityEntry = _dbContext.Entry(oldOwnerData);
        
        // Check if entity has changed
        Console.WriteLine(entityEntry.State == EntityState.Modified
            ? "Owner data has been changed."
            : "Owner data wasn't changed.");
    }
}