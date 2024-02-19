using FakeItEasy;
using FluentValidation;
using MessageServer.Domain;
using MessageServer.Infrastructure.Repositories.Abstractions;

namespace ServerTests;

public class PetRepoTest
{
    [Fact]
    public async Task CreateAsync_ValidPetDtoInput_CreatesInDataBase()
    {
        var pet = new Pet
        {
            Id = new Guid(), Name = "DeathMetal", PetAge = 5
        };
        var petValidator = new PetValidator();
        var fPetRepo = A.Fake<IPetRepository>();
        
        await fPetRepo.CreateAsync(pet: pet);
        
        await petValidator.ValidateAndThrowAsync(pet);
        A.CallTo(() => fPetRepo.CreateAsync(A<Pet>._)).MustHaveHappenedOnceExactly();
    }
}