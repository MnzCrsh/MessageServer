using FakeItEasy;
using FluentValidation;
using MessageServer.Domain;
using MessageServer.Infrastructure;

namespace ServerTests;

public class PetRepoTest
{
    [Fact]
    public async Task CreateAsync_ValidPetDtoInput_CreatesInDataBase()
    {
        var petDto = new PetDto
        {
            Id = new Guid(), Name = "DeathMetal", PetAge = 100
        };
        var petValidator = new PetDtoValidator();
        var fPetRepo = A.Fake<IPetRepository>();
        
        await fPetRepo.CreateAsync(pet: petDto);
        
        await petValidator.ValidateAndThrowAsync(petDto);
        A.CallTo(() => fPetRepo.CreateAsync(A<PetDto>._)).MustHaveHappenedOnceExactly();
    }
}