using MessageServer.Domain;

namespace MessageServer.Infrastructure;

public static class OwnerMapper
{
    public static OwnerDto EntityToDto(Owner? owner)
    {
        var dto = new OwnerDto
        {
            Id = owner.Id,
            Name = owner.Name,
            OwnedPets = PetMapper.EntityToDto(owner.OwnedPets)
        };
        return dto;
    }
    
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