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

    public async Task<IEnumerable<PetDto>> GetAllAsync()
    {
        var result = await (_dbContext.Pets ?? throw new InvalidOperationException())
            .Where(p => !p.IsMarkedToDelete)
            .Select(p => new PetDto
            {
                Id = p.Id,
                Name = p.Name,
                PetOwner = new OwnerDto{Id = p.PetOwner!.Id, Name = p.PetOwner.Name}
            })
            .ToListAsync();
        return result;
    }

    public Task UpdateAsync(Pet pet)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id, bool confirmDelete = false)
    {
        throw new NotImplementedException();
    }
}