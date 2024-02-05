using MessageServer.Domain;

namespace MessageServer.Infrastructure;

public class PetRepository : IPetRepository
{
    public PetRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    private readonly PostgresDbContext _dbContext;

    public async Task<Guid> CreateAsync(PetDto pet)
    {
        var newPet = new PetDto
        {
            Id = new Guid(),
            Name = pet.Name,
            PetAge = pet.PetAge
        };
        _dbContext.Add(newPet);
        await _dbContext.SaveChangesAsync();
        return newPet.Id;
    }

    public Task<PetDto> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PetDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(PetDto pet)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}