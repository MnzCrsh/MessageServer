using MessageServer.Infrastructure.Repositories.Interfaces;

namespace MessageServer.Infrastructure.Repositories.Implementations;

public static class PetOwnerRepositoryExtension
{
    public static async Task AddPetToOwnerAsync(this IPetOwnerRepository ownerRepo,
                                                     IPetRepository petRepo,int ownerId, int petId)
    {
        var owner = await ownerRepo.GetAsync(ownerId);
        var pet = await petRepo.GetAsync(petId);
        owner.OwnedPets?.Add(pet);
    }

    public static async Task RemovePetFromOwnerAsync(this IPetOwnerRepository ownerRepo,
        IPetRepository petRepo,int ownerId, int petId)
    {
        var owner = await ownerRepo.GetAsync(ownerId);
        var pet = await petRepo.GetAsync(petId);
        owner.OwnedPets?.Remove(pet);
    }
}