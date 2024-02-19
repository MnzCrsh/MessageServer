using MessageServer.Domain;
using MessageServer.Infrastructure.Repositories.Abstractions;

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
        _dbContext.Add(newPet);
        await _dbContext.SaveChangesAsync();
        return newPet.Id;
    }

    public Task<Pet> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PetDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Pet pet)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}