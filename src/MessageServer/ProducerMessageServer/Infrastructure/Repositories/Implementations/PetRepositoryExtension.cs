using MessageServer.Domain;

namespace MessageServer.Infrastructure.Repositories.Implementations;

public static class PetRepositoryExtension
{
    public static async Task<IEnumerable<PetDto>> GetPetsByOwnerAsync(this PetRepository repo,int ownerId)
    {
        throw new NotImplementedException();
    }
}