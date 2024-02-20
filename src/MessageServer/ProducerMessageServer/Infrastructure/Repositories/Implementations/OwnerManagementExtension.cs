using MessageServer.Domain;
using MessageServer.Infrastructure.Repositories.Abstractions;

namespace MessageServer.Infrastructure.Repositories.Implementations;


public static class OwnerManagementExtension
{
    //HACK: Static events. Maybe should use the Observer pattern.
    public static event EventHandler<OwnerEventArgs>? OnPetAdded;
    public static event EventHandler<OwnerEventArgs>? OnPetRemoved;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerRepo">Repository interface</param>
    /// <param name="existingPet">Existing pet to add</param>
    /// <param name="existingOwner">Existing owner to who pet will be added</param>
    public static void AddPet(this IOwnerRepository ownerRepo, Owner existingOwner, Pet existingPet)
    {
        existingOwner.OwnedPets?.Add(existingPet);
        OnPetAdded?.Invoke(ownerRepo, new OwnerEventArgs(existingOwner));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerRepo">Repository interface</param>
    /// <param name="existingPet">Existing pet to remove</param>
    /// <param name="existingOwner">Existing owner from who pet will be removed</param>
    public static void RemovePet(this IOwnerRepository ownerRepo, Owner existingOwner, Pet existingPet)
    {
        existingOwner.OwnedPets?.Remove(existingPet);
        OnPetRemoved?.Invoke(ownerRepo, new OwnerEventArgs(existingOwner));
    }
}