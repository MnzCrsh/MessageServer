using MessageServer.Domain;
using MessageServer.Infrastructure.Repositories.Abstractions;

namespace MessageServer.Infrastructure.Repositories.Implementations;

public static class PetRepositoryExtension
{
    public static async Task<IEnumerable<PetDto>> GetPetsByOwnerAsync(this IPetRepository repo,Guid ownerId)
    {
        var pets = await repo.GetAllAsync();
        return pets.Where(pet => pet.PetOwner != null && pet.PetOwner.Id.Equals(ownerId)).Select(p => p).ToList();
    }
}