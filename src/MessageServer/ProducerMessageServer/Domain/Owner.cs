﻿namespace MessageServer.Domain;

public class Owner
{
    public required Guid Id                       { get; set; }
    
    public required string Name                   { get; set; } = null!;

    public bool IsMarkedToDelete                  { get; set; }
    
    public ICollection<Pet>? OwnedPets         { get; set; }
}