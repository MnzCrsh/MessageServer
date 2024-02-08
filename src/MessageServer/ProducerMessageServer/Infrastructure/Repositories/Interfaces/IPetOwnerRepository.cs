using MessageServer.Domain;

namespace MessageServer.Infrastructure.Repositories.Interfaces;

public interface IPetOwnerRepository
{
    public Task<Guid> CreateAsync(PetOwnerDto owner);
    public Task<PetOwnerDto> GetAsync(int id);
    public Task<IEnumerable<PetOwnerDto>> GetAllAsync();
    public Task UpdateAsync(PetOwnerDto owner);
    public Task DeleteAsync(int id);
}