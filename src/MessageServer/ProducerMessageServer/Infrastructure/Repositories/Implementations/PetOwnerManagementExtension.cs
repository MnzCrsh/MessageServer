using MessageServer.Domain;
using MessageServer.Infrastructure.Repositories.Interfaces;

namespace MessageServer.Infrastructure.Repositories.Implementations;


public static class PetOwnerManagementExtension
{

    //HACK: Static events. Maybe should try the library for weak events or should use the weak references.
    public static event EventHandler<OwnerEventArgs>? PetAdded;
    public static event EventHandler<OwnerEventArgs>? PetRemoved;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerRepo"></param>
    /// <param name="existingPet"></param>
    /// <param name="existingOwner"></param>
    public static void AddPet(this IOwnerRepository ownerRepo, Pet existingPet,Owner existingOwner)
    {
        existingOwner.OwnedPets?.Add(existingPet);
        PetAdded?.Invoke(ownerRepo, new OwnerEventArgs(existingOwner));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerRepo"></param>
    /// <param name="existingPet"></param>
    /// <param name="existingOwner"></param>
    public static void RemovePet(this IOwnerRepository ownerRepo, Pet existingPet, Owner existingOwner)
    {
        existingOwner.OwnedPets?.Remove(existingPet);
        PetRemoved?.Invoke(ownerRepo, new OwnerEventArgs(existingOwner));
    }
}