using FakeItEasy;
using MessageServer.Domain;
using MessageServer.Infrastructure;

namespace ServerTests;

public class PetRepoTest
{
    [Fact]
    public async Task CreateAsync_ReturnsValue()
    {
        var fPet = A.Dummy<PetDto>();
        var fDbContext = A.Fake<PostgresDbContext>();
        var petRepo = new PetRepository(fDbContext);
        
        //A.CallTo(() => fDbContext.Add(A<PetDto>.Ignored)).Returns(fPet);

        var result = await petRepo.CreateAsync(fPet);
        
        Assert.NotNull(result);
        A.CallTo(() => fDbContext.Add(A<PetDto>.Ignored)).MustHaveHappened(); 
    }
}