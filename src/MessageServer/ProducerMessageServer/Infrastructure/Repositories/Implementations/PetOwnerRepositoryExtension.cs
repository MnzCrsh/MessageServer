using MessageServer.Domain;
using MessageServer.Infrastructure.Repositories.Interfaces;

namespace MessageServer.Infrastructure.Repositories.Implementations;

public static class PetOwnerRepositoryExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerRepo"></param>
    /// <param name="petRepo"></param>
    /// <param name="ownerId"></param>
    /// <param name="petId"></param>
    public static async Task AddPetAsync(this IOwnerRepository ownerRepo,
                                              IPetRepository petRepo,int ownerId, int petId)
    {
        var (owner,pet) = await GetOwnerAndPetAsync(ownerRepo, petRepo, ownerId, petId);
        owner.OwnedPets?.Add(pet);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerRepo"></param>
    /// <param name="petRepo"></param>
    /// <param name="ownerId"></param>
    /// <param name="petId"></param>
    public static async Task RemovePetAsync(this IOwnerRepository ownerRepo,
                                                 IPetRepository petRepo,int ownerId, int petId)
    {
        var (owner,pet) = await GetOwnerAndPetAsync(ownerRepo, petRepo, ownerId, petId);
        owner.OwnedPets?.Remove(pet);
    }
    
    //HACK:
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerRepo"></param>
    /// <param name="petRepo"></param>
    /// <param name="ownerId"></param>
    /// <param name="petId"></param>
    /// <returns></returns>
    private static async Task<(Owner, Pet)> GetOwnerAndPetAsync(
        this IOwnerRepository ownerRepo, IPetRepository petRepo, int ownerId, int petId)
    {
        var owner = await ownerRepo.GetAsync(ownerId);
        var pet = await petRepo.GetAsync(petId);
        return (owner, pet);
    }
}