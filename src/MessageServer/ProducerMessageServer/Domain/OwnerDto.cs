namespace MessageServer.Domain;

public record OwnerDto
{
    public required Guid Id                       { get; set; }
    
    public required string Name                   { get; set; } = null!;

    public bool IsMarkedToDelete                  { get; set; }
    
    public ICollection<PetDto>? OwnedPets         { get; set; }
}