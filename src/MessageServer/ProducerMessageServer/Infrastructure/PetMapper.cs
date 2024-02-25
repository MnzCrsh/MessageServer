using MessageServer.Domain;

namespace MessageServer.Infrastructure;

public static class PetMapper
{
    public static PetDto EntityToDto(Pet pet)
    {
        var dto = new PetDto
        {
            Id = pet.Id,
            PetOwner = OwnerMapper.EntityToDto(pet.PetOwner ??
                                               throw new ArgumentNullException(nameof(pet.PetOwner))),
            Name = pet.Name
        };
        return dto;
    } 
    public static IEnumerable<PetDto> EntityToDto(IEnumerable<Pet>? pets)
    {
        return (pets ?? throw new ArgumentNullException(nameof(pets)))
            .Select(pet => new PetDto
            {
                Id = pet.Id,
                PetOwner = OwnerMapper.EntityToDto(pet.PetOwner ?? 
                                                   throw new ArgumentNullException(nameof(pet.PetOwner))),
                Name = pet.Name
            })
            .ToList();
    }
}