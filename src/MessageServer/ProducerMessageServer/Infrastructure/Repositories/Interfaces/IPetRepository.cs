using MessageServer.Domain;

namespace MessageServer.Infrastructure.Repositories.Interfaces;

public interface IPetRepository
{
    public Task<Guid> CreateAsync(PetDto pet);
    public Task<PetDto> GetAsync(int id);
    public Task<IEnumerable<PetDto>> GetAllAsync();
    public Task UpdateAsync(PetDto pet);
    public Task DeleteAsync(int id);
}