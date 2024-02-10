namespace MessageServer.Domain;

public record OwnerDto
{
    public required Guid Id                       { get; set; }
    
    public required string Name                   { get; set; } = null!;
    
    public virtual IEnumerable<PetDto>? OwnedPets         { get; set; }
}