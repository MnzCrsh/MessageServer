using MessageServer.Domain;
using MessageServer.Infrastructure.Repositories.Interfaces;

namespace MessageServer.Infrastructure.Repositories.Implementations;


public class PetOwnerRepository : IPetOwnerRepository
{
    public PetOwnerRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    private readonly PostgresDbContext _dbContext;
    
    public async Task<Guid> CreateAsync(PetOwnerDto owner)
    {
        PetOwnerDto newOwner = new PetOwnerDto
        {
            Id = new Guid(),
            Name = owner.Name,
            IsMarkedToDelete = false
        };
        _dbContext.Add(newOwner);
        await _dbContext.SaveChangesAsync();
        return newOwner.Id;
    }

    public async Task<PetOwnerDto> GetAsync(int id)
    {
        //TODO: To change
        var owner = new PetOwnerDto { Name = "", Id = Guid.Empty};
        return owner;
    }

    public Task<IEnumerable<PetOwnerDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(PetOwnerDto owner)
    {
        throw new NotImplementedException();
    }
    

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}