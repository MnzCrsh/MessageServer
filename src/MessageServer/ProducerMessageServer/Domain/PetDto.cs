using System.ComponentModel.DataAnnotations;

namespace MessageServer.Domain;

public record PetDto
{
    [Required]
    public Guid Id               { get; set; }
    
    public OwnerDto? PetOwner { get; set; }
    
    public required string Name  { get; set; } = null!;
}