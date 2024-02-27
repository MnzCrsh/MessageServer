using MessageServer.Domain;
using MessageServer.Infrastructure.Repositories.Abstractions;

namespace MessageServer.Infrastructure.Repositories.Implementations;

public static class PetRepositoryExtension
{
    public static async Task<IEnumerable<PetDto>> GetPetsByOwnerAsync(this IPetRepository repo, Guid ownerId,
        CancellationToken ct = default)
    {
        var pets = await repo.GetAllAsync(ct);
        
        if (pets == null) throw new ArgumentNullException(nameof(ownerId),
            $"There is no pets belonging to ID: {ownerId}");
        
        return pets.Where(pet => pet.PetOwner != null && pet.PetOwner.Id
            .Equals(ownerId))
            .Select(p => p)
            .ToList();
    }
}