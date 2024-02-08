using System.ComponentModel.DataAnnotations;

namespace MessageServer.Domain;

public record PetDto
{
    [Required]
    public Guid Id               { get; set; }
    
    public PetOwnerDto? PetOwner { get; set; }
    
    public required string Name  { get; set; } = null!;
    
    [Required, Range(1,100)]
    public int PetAge            { get; set; }
    
    public bool IsMarkedToDelete { get; set; }
}