namespace MessageServer.Domain;

public record PetOwnerDto
{
    public Guid Id                                { get; set; }
    
    public required string Name                   { get; set; } = null!;
    
    public bool IsMarkedToDelete                  { get; set; }
    
    public required ICollection<PetDto> OwnedPets { get; set; } = null!;
}