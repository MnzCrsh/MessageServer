using MessageServer.Domain;

namespace MessageServer.Infrastructure.Repositories.Interfaces;

public interface IOwnerRepository
{
    public Task<Guid> CreateAsync(Owner owner);
    public Task<Owner> GetAsync(int id);
    public Task<IEnumerable<OwnerDto>> GetAllAsync();
    public Task UpdateAsync(Owner owner);
    public Task DeleteAsync(int id);
}