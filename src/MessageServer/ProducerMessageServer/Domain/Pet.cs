using System.ComponentModel.DataAnnotations;

namespace MessageServer.Domain;

public class Pet
{
        [Required]
        public Guid Id               { get; set; }
    
        public Owner? PetOwner { get; set; }
    
        public required string Name  { get; set; } = null!;
    
        [Required, Range(1,100)]
        public int PetAge            { get; set; }
    
        public bool IsMarkedToDelete { get; set; }
}