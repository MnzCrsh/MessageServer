using MessageServer.Domain;

namespace MessageServer.Infrastructure;

public static class OwnerMapper
{
    /// <summary>
    /// Maps single db entity to data transfer object
    /// </summary>
    /// <param name="owner">Owner data base entity</param>
    /// <returns></returns>
    public static OwnerDto EntityToDto(Owner owner)
    {
        var dto = new OwnerDto
        {
            Id = owner.Id,
            Name = owner.Name,
            OwnedPets = PetMapper.EntityToDto(owner.OwnedPets)
        };
        return dto;
    }
    
    /// <summary>
    /// Maps multiple db entities to data transfer objects
    /// </summary>
    /// <param name="owners">List of owners</param>
    /// <returns></returns>
    public static IEnumerable<OwnerDto> EntityToDto(IEnumerable<Owner> owners)
    {
        var dto = owners.Select(owner => new OwnerDto
        {
            Id = owner.Id,
            Name = owner.Name,
            OwnedPets = PetMapper.EntityToDto(owner.OwnedPets)
        }).ToList();
        return dto;
    }
}