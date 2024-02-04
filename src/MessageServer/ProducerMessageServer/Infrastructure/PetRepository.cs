using MessageServer.Domain;

namespace MessageServer.Infrastructure;

public class PetRepository : IPetRepository
{
    public Task CreateAsync(PetDto pet)
    {
        throw new NotImplementedException();
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