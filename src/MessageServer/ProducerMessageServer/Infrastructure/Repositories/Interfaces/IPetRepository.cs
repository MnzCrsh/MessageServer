using MessageServer.Domain;

namespace MessageServer.Infrastructure.Repositories.Interfaces;

public interface IPetRepository
{
    public Task<Guid> CreateAsync(Pet pet);
    public Task<Pet> GetAsync(int id);
    public Task<IEnumerable<PetDto>> GetAllAsync();
    public Task UpdateAsync(Pet pet);
    public Task DeleteAsync(int id);
}