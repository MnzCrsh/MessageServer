using FakeItEasy;
using FluentValidation;
using MessageServer.Domain;
using MessageServer.Infrastructure.Repositories.Interfaces;

namespace ServerTests;

public class PetRepoTest
{
    [Theory]
    [InlineData(0),
     InlineData(10),
     InlineData(100),
     InlineData(1000)]
    public async Task CreateAsync_ValidPetDtoInput_CreatesInDataBase(int petAge)
    {
        var pet = new Pet
        {
            Id = new Guid(), Name = "DeathMetal", PetAge = petAge
        };
        var petValidator = new PetValidator();
        var fPetRepo = A.Fake<IPetRepository>();
        
        await fPetRepo.CreateAsync(pet: pet);
        
        await petValidator.ValidateAndThrowAsync(pet);
        A.CallTo(() => fPetRepo.CreateAsync(A<Pet>._)).MustHaveHappenedOnceExactly();
    }
}