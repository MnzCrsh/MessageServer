using FakeItEasy;
using FluentAssertions;
using MessageServer.Domain;
using MessageServer.Infrastructure;
using MessageServer.Infrastructure.Repositories;
using MessageServer.Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace ServerTests;

public class OwnerRepoTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateOwner_ShouldReturnNewOwnerId()
    {
        //Arrange
        var fOwner =  new Owner
        {
            Id = Guid.Empty,
            Name = "name",
            PassportSeries = 0123,
            PassportNumber = 456789,
            IsMarkedToDelete = false,
            OwnedPets = null
        };
        
        var fCbf = A.Fake<CircuitBreaker.CircuitBreakerFactory>();
        var options = new DbContextOptionsBuilder<PostgresDbContext>()
            .UseInMemoryDatabase(databaseName: "test")
            .Options;
        var dbContext = new PostgresDbContext(options);
        
        var repository = new OwnerRepository(dbContext, fCbf);

        //Act
        var result = await repository.CreateAsync(fOwner);
        
        //Assert
        fOwner.Id = result;
        dbContext.PetOwners.Any(o=> o.Id == result).Should().BeTrue();
        result.Should().Be(fOwner.Id);
    }
}