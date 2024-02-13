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
        var fOwner = CreateOwner();
        var fCbf = A.Fake<CircuitBreaker.CircuitBreakerFactory>();

        Guid result;
        await using (var dbContext = GetInMemoryDbContext())
        {
            var repository = new OwnerRepository(dbContext, fCbf);

            //Act
            result = await repository.CreateAsync(fOwner);

            //Assert
            fOwner.Id = result;
            dbContext.PetOwners.Any(o => o.Id == result).Should().BeTrue();
        }

        result.Should().Be(fOwner.Id);
    }
    
    //TODO: Add exception path
    
    [Fact]
    public async Task GetAsync_Should_Return_Owner_By_Valid_Id()
    {
        //Arrange
        var expectedOwner = CreateOwner();
        var fCbf = A.Fake<CircuitBreaker.CircuitBreakerFactory>();

        
        await using var dbContext = GetInMemoryDbContext();
        
        await dbContext.AddAsync(expectedOwner);
        await dbContext.SaveChangesAsync();

        var repository = new OwnerRepository(dbContext, fCbf);
        
        //Act
        var result = await repository.GetAsync(expectedOwner.Id);
        
        //Assert
        result.Id.Should().Be(expectedOwner.Id);
    }
    
    //TODO: Add exception path

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllOwnerIEnumerable_WhenOwnerExist()
    {
        //Arrange
        var fCbf = A.Fake<CircuitBreaker.CircuitBreakerFactory>();

        var dbContext = GetInMemoryDbContext();
        var repository = new OwnerRepository(dbContext,fCbf);
        
        var expectedOwners = new List<Owner>();
        for (int i = 0; i < 5; i++)
        {
            var owner = CreateOwner();
            expectedOwners.Add(owner);
        }

        await dbContext.AddAsync(expectedOwners);

        //TODO: MAP OWNER TO OWNER DTO
        //Act
        var result = await repository.GetAllAsync();

        //Arrange
        result.Should().OnlyContain(dto => expectedOwners.Any( ));
    }
    
    private static PostgresDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<PostgresDbContext>()
            .UseInMemoryDatabase(databaseName: "test")
            .Options;
        var dbContext = new PostgresDbContext(options);
        return dbContext;
    }
    
    private static Owner CreateOwner()
    {
        var fOwner = new Owner
        {
            Id = Guid.NewGuid(),
            Name = "name",
            PassportSeries = 0123,
            PassportNumber = 456789,
            IsMarkedToDelete = false,
            OwnedPets = null
        };
        return fOwner;
    }  
    private static OwnerDto CreateOwnerDto()
    {
        var fDto = new OwnerDto()
        {
            Id = Guid.NewGuid(),
            Name = "name",
            OwnedPets = null
        };
        return fDto;
    }
}