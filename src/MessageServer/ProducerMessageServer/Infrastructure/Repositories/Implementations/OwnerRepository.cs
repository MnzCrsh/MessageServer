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
    
    //TODO: Delegate owner to factory/ Maybe hash passport data
    /// <summary>
    /// Asynchronously creates new Owner in database. Adds to database with DbContext
    /// </summary>
    /// <param name="owner">Owner entity</param>
    /// <returns></returns>
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

    /// <summary>
    /// Asynchronously returns Owner from data base
    /// </summary>
    /// <param name="id">ID of the existing Owner</param>
    /// <returns>Existing Owner from DB</returns>
    /// <exception cref="InvalidOperationException">Owner with such ID dont exist in DB</exception>
    public async Task<Owner> GetAsync(int id)
    {
        var existingOwner = await _dbContext.PetOwners
            .Where(o => o.Id.Equals(id) && !o.IsMarkedToDelete)
            .Select(o => o)
            .SingleOrDefaultAsync() ?? 
                            throw new InvalidOperationException($"{id} dont exist");
        return existingOwner;
    }

    /// <summary>
    /// Asynchronously returns every owner from DB with linked pets
    /// </summary>
    /// <returns>Owners IEnumerable</returns>
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

    /// <summary>
    /// Asynchronously updates old Owner info to new Owner info
    /// </summary>
    /// <param name="owner">New Owner data with the same ID</param>
    /// <param name="updateCallback"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task UpdateAsync(Owner owner, Action<Owner,Owner> updateCallback)
    {
        var oldOwnerData = await _dbContext.PetOwners
            .FirstOrDefaultAsync(o => o.Id.Equals(owner.Id));
        
        UpdateOwnerDataCallback(oldOwnerData ?? throw new ArgumentNullException
            ($"Cant find owner with ID: {owner.Id}"), owner);

        await _dbContext.SaveChangesAsync();
    }
    

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="oldOwnerData"></param>
    /// <param name="newOwnerData"></param>
    private void UpdateOwnerDataCallback(Owner oldOwnerData, Owner newOwnerData)
    {
        oldOwnerData.Name = newOwnerData.Name;
        oldOwnerData.PassportNumber = newOwnerData.PassportNumber;
        oldOwnerData.PassportSeries = newOwnerData.PassportSeries;
        if (oldOwnerData.IsMarkedToDelete && newOwnerData.IsMarkedToDelete.Equals(false))
        {
            oldOwnerData.IsMarkedToDelete = newOwnerData.IsMarkedToDelete;
        }
        
        // Get information about entity state
        var entityEntry = _dbContext.Entry(oldOwnerData);
        
        // Check if entity has changed
        Console.WriteLine(entityEntry.State == EntityState.Modified
            ? "Owner data has been changed."
            : "Owner data wasn't changed.");
    }
}