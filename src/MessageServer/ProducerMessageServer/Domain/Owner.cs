using System.ComponentModel.DataAnnotations;

namespace MessageServer.Domain;

public class Owner
{
    public required Guid Id                       { get; set; }
    
    public required string Name                   { get; set; } = null!;
    
    [Range(4,4)]
    public required int PassportSeries            { get; set; }
    [Range(6,6)]
    public required int PassportNumber            { get; set; }

    public bool IsMarkedToDelete                  { get; set; }
    
    public virtual ICollection<Pet>? OwnedPets    { get; set; }
}