using FakeItEasy;
using FluentValidation;
using MessageServer.Domain;
using MessageServer.Infrastructure.Repositories;
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
        var petDto = new PetDto
        {
            Id = new Guid(), Name = "DeathMetal", PetAge = petAge
        };
        var petValidator = new PetDtoValidator();
        var fPetRepo = A.Fake<IPetRepository>();
        
        await fPetRepo.CreateAsync(pet: petDto);
        
        await petValidator.ValidateAndThrowAsync(petDto);
        A.CallTo(() => fPetRepo.CreateAsync(A<PetDto>._)).MustHaveHappenedOnceExactly();
    }
}