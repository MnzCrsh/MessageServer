using MessageServer.Domain;
using MessageServer.Infrastructure.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MessageServer.Infrastructure.Repositories.Implementations;

public class PetRepository : IPetRepository
{
    public PetRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    private readonly PostgresDbContext _dbContext;

    public async Task<Guid> CreateAsync(Pet pet)
    {
        var newPet = new Pet
        {
            Id = new Guid(),
            Name = pet.Name,
            PetAge = pet.PetAge,
            IsMarkedToDelete = false
        };
        await _dbContext.AddAsync(newPet);
        await _dbContext.SaveChangesAsync();
        return newPet.Id;
    }

    public async Task<Pet?> GetAsync(Guid id)
    {
        var result = await (_dbContext.Pets ?? throw new InvalidOperationException())
            .Where(p => !p.IsMarkedToDelete)
            .Select(p => p)
            .SingleOrDefaultAsync();
        return result;
    }

    public async Task<IEnumerable<PetDto>> GetAllAsync(CancellationToken ct = default)
    {
        var result = await (_dbContext.Pets ?? throw new InvalidOperationException())
            .Where(p => !p.IsMarkedToDelete)
            .Select(p => new PetDto
            {
                Id = p.Id,
                Name = p.Name,
                PetOwner = new OwnerDto{Id = p.PetOwner!.Id, Name = p.PetOwner.Name}
            })
            .ToListAsync(cancellationToken: ct);
        return result;
    }

    public async Task UpdateAsync(Pet newPetData)
    {
        var oldPetData = await _dbContext.Pets.FirstOrDefaultAsync(pet => pet.Id.Equals(newPetData.Id));
        if (oldPetData is null)
        {
            throw new ArgumentNullException($"Cant find owner with ID: {newPetData.Id}");
        }
        
        UpdatePetData(oldPetData, newPetData);
    }

    public async Task DeleteAsync(Guid id, bool confirmDelete = false)
    {
        var pet = await _dbContext.Pets.FindAsync(id);
        if (pet is null) throw new ArgumentNullException
            ($"Pet with ID: {id} does not exist");
        
        if (!pet.IsMarkedToDelete) pet.IsMarkedToDelete = true;
        else
        {
            if (confirmDelete)
            {
                _dbContext.Remove(pet);
            }
            else
            {
                throw new InvalidOperationException
                    ("Confirmation was not received");
            }
        }
        
        await _dbContext.SaveChangesAsync();
    }

    private static void UpdatePetData(Pet oldPetData, Pet newPetData)
    {
        oldPetData.Name = newPetData.Name;
        oldPetData.PetAge = newPetData.PetAge;
        if (oldPetData.IsMarkedToDelete && newPetData.IsMarkedToDelete.Equals(false))
        {
            oldPetData.IsMarkedToDelete = newPetData.IsMarkedToDelete;
        }
    }
}