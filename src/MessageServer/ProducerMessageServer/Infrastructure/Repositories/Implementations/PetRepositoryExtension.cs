using MessageServer.Domain;
using MessageServer.Infrastructure.Repositories.Abstractions;

namespace MessageServer.Infrastructure.Repositories.Implementations;

public static class PetRepositoryExtension
{
    public static Task<IEnumerable<PetDto>> GetPetsByOwnerAsync(this IPetRepository repo,int ownerId)
    {
        throw new NotImplementedException();
    }
}