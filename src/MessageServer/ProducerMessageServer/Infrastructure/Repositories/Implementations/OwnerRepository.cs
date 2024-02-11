using MessageServer.Domain;
using MessageServer.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MessageServer.Infrastructure.Repositories.Implementations;


public class OwnerRepository : IOwnerRepository
{
    private readonly PostgresDbContext _dbContext;
    private readonly CircuitBreaker _circuitBreaker;
    public OwnerRepository(PostgresDbContext dbContext,
        CircuitBreaker.CircuitBreakerFactory circuitBreakerFactory)
    {
        _dbContext = dbContext;
        _circuitBreaker = circuitBreakerFactory(TimeSpan.FromSeconds(5));
    }
    
    
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
    /// Asynchronously updates OLD Owner data to the NEW Owner data
    /// </summary>
    /// <param name="newOwnerData">New Owner data with the same ID</param>
    /// <exception cref="ArgumentNullException">Throws if owner with such ID don't exist in DB</exception>
    public async Task UpdateAsync(Owner newOwnerData)
    {
        var oldOwnerData = await _dbContext.PetOwners
            .FirstOrDefaultAsync(o => o.Id.Equals(newOwnerData.Id));
        
        if (oldOwnerData is null)
        {
            throw new ArgumentNullException($"Cant find owner with ID: {newOwnerData.Id}");
        }
        
        UpdateOwnerData(oldOwnerData,newOwnerData);

        await _dbContext.SaveChangesAsync();
    }
    
    /// <summary>
    /// Asynchronously updates OLD Owner data to the NEW Owner data
    /// </summary>
    /// <param name="newOwnerData">New Owner data with the same ID</param>
    /// <param name="updateCallback">Update delegate</param>
    /// <exception cref="ArgumentNullException">Throws if owner with such ID don't exist in DB</exception>
    public async Task UpdateAsync(Owner newOwnerData, Action<Owner, Owner>? updateCallback)
    {
        var oldOwnerData = await _dbContext.PetOwners
            .FirstOrDefaultAsync(o => o.Id.Equals(newOwnerData.Id));
        
        if (oldOwnerData is null)
        {
            throw new ArgumentNullException($"Cant find owner with ID: {newOwnerData.Id}");
        }
        
        _circuitBreaker.Execute(()=>updateCallback?.Invoke(oldOwnerData,newOwnerData));

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Marks owner to delete
    /// </summary>
    /// <param name="id">ID of the Owner to delete</param>
    /// <exception cref="ArgumentNullException">Throws if owner dont exist in DB</exception>
    public async Task DeleteAsync(int id)
    {
        var owner = await _dbContext.PetOwners.FindAsync(id);
        if (owner is null)
        {
            throw new ArgumentNullException($"Owner with ID: {id} does not exist");
        }

        owner.IsMarkedToDelete = true;
        await _dbContext.SaveChangesAsync();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">ID of the Owner to delete</param>
    /// <param name="deleteCallback">Delete delegate</param>
    /// <exception cref="ArgumentNullException">Throws if owner dont exist in DB</exception>
    public async Task DeleteAsync(int id, Action<Owner>? deleteCallback)
    {
        var owner = await _dbContext.PetOwners.FindAsync(id);
        if (owner is null)
        {
            throw new ArgumentNullException($"Owner with ID: {id} does not exist");
        }

        _circuitBreaker.Execute(()=> deleteCallback?.Invoke(owner));
        await _dbContext.SaveChangesAsync();
    }   
    

    /// <summary>
    /// Default update callback implementation. Changes old data to the new.
    /// Also checks if data was or wasn't changed already.
    /// </summary>
    /// <param name="oldOwnerData">Data to change</param>
    /// <param name="newOwnerData">New data that'll replace old one</param>
    private void UpdateOwnerData(Owner oldOwnerData, Owner newOwnerData)
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