using MessageServer.Domain;
using MessageServer.Infrastructure.Repositories.Interfaces;

namespace MessageServer.Infrastructure.Repositories.Implementations;

public static class PetOwnerRepositoryExtension
{
    public static async Task AddPetAsync(this IOwnerRepository ownerRepo,
                                              IPetRepository petRepo,int ownerId, int petId)
    {
        var (owner,pet) = await GetOwnerAndPetAsync(ownerRepo, petRepo, ownerId, petId);
        owner.OwnedPets?.Add(pet);
    }

    public static async Task RemovePetAsync(this IOwnerRepository ownerRepo,
                                                 IPetRepository petRepo,int ownerId, int petId)
    {
        var (owner,pet) = await GetOwnerAndPetAsync(ownerRepo, petRepo, ownerId, petId);
        owner.OwnedPets?.Remove(pet);
    }
    
    //HACK:
    private static async Task<(Owner, Pet)> GetOwnerAndPetAsync(
        this IOwnerRepository ownerRepo, IPetRepository petRepo, int ownerId, int petId)
    {
        var owner = await ownerRepo.GetAsync(ownerId);
        var pet = await petRepo.GetAsync(petId);
        return (owner, pet);
    }
}