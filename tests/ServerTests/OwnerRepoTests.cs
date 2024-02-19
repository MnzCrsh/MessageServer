using FakeItEasy;
using FluentAssertions;
using MessageServer.Domain;
using MessageServer.Infrastructure;
using MessageServer.Infrastructure.Repositories;
using MessageServer.Infrastructure.Repositories.Abstractions;
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
        var fStrategy = A.Fake<IOwnerEventStrategy>();

        Guid result;
        await using (var dbContext = GetInMemoryDbContext())
        {
            var repository = new OwnerRepository(dbContext, fCbf,fStrategy);

            //Act
            result = await repository.CreateAsync(fOwner);

            //Assert
            fOwner.Id = result;
            dbContext.PetOwners.Any(o => o.Id == result).Should().BeTrue();
        }
        result.Should().Be(fOwner.Id);
    }
    
    //TODO: Add the exception path
    
    [Fact]
    public async Task GetAsync_Should_Return_Owner_By_Valid_Id()
    {
        //Arrange
        var expectedOwner = CreateOwner();
        var fCbf = A.Fake<CircuitBreaker.CircuitBreakerFactory>();
        var fStrategy = A.Fake<IOwnerEventStrategy>();
        
        
        await using var dbContext = GetInMemoryDbContext();
        
        await dbContext.AddAsync(expectedOwner);
        await dbContext.SaveChangesAsync();

        var repository = new OwnerRepository(dbContext, fCbf,fStrategy);
        
        //Act
        var result = await repository.GetAsync(expectedOwner.Id);
        
        //Assert
        result.Id.Should().Be(expectedOwner.Id);
    }
    
    //TODO: Add the exception path

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllOwnerIEnumerable_WhenOwnerExist()
    {
        //Arrange
        var fCbf = A.Fake<CircuitBreaker.CircuitBreakerFactory>();
        var fStrategy = A.Fake<IOwnerEventStrategy>();
        
        await using var dbContext = GetInMemoryDbContext();
        var repository = new OwnerRepository(dbContext, fCbf,fStrategy);

        List<Owner> expectedOwners = new();
        for (int i = 0; i < 5; i++)
        {
            var owner = CreateOwner();
            expectedOwners.Add(owner);
            await dbContext.AddAsync(owner);
        }
        await dbContext.SaveChangesAsync();
        
        // ReSharper disable once IdentifierTypo
        var expectedOwnersDtos = expectedOwners.Select(o => new OwnerDto
        {
            Id = o.Id,
            Name = o.Name,
            OwnedPets = null
        }).ToList();
        
        //Act
        var result = await repository.GetAllAsync();

        //Assert
        result.Should().OnlyContain(dto => expectedOwnersDtos.Any(o => o.Id == dto.Id));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesExistingOwner()
    {
        //Arrange
        var fCbf = A.Fake<CircuitBreaker.CircuitBreakerFactory>();
        var fStrategy = A.Fake<IOwnerEventStrategy>();

        var existingOwner = CreateOwner();
        var newOwner = CreateOwner(existingOwner.Id,"newName", 3456, 789483, false);

        await using (var dbContext = GetInMemoryDbContext())
        {
            var repository = new OwnerRepository(dbContext, fCbf,fStrategy);
            await dbContext.AddAsync(existingOwner);
            await dbContext.SaveChangesAsync();
            
            //Act
            await repository.UpdateAsync(newOwner);
        }
        
        //Assert
        await using (var context = GetInMemoryDbContext())
        {
            var changedOwner = await context.PetOwners.FindAsync(existingOwner.Id);
                changedOwner.Should().BeEquivalentTo(newOwner);
        }
    }
    
    //TODO: Add the exception path

    [Fact]
    public async Task DeleteAsync_WhenOwnerExistsAndNotMarkedToDelete_MarksOwnerAsDeleted()
    {
        //Arrange
        var owner = CreateOwner();

        await using (var dbContext = GetInMemoryDbContext())
        {
            var fCbf = A.Fake<CircuitBreaker.CircuitBreakerFactory>();
            var fStrategy = A.Fake<IOwnerEventStrategy>();
            
            var repository = new OwnerRepository(dbContext, fCbf,fStrategy);

            await dbContext.AddAsync(owner);
            await dbContext.SaveChangesAsync();
            
            //Act
            await repository.DeleteAsync(owner.Id);
        }


        //Assert
        await using (var dbContext = GetInMemoryDbContext())
        {
            var markedOwner = await dbContext.PetOwners.FindAsync(owner.Id);
            markedOwner?.IsMarkedToDelete.Should().Be(true);
        }
    }

    [Fact]
    public async Task DeleteAsync_WhenOwnerExistsAndMarkedToDelete_AndConfirmDeleteIsTrue_RemovesOwner()
    {
        //Arrange
        var owner = CreateOwner(Guid.NewGuid(), "ボリス", 1234, 56789, true);
        
        var fCbf = A.Fake<CircuitBreaker.CircuitBreakerFactory>();
        var fStrategy = A.Fake<IOwnerEventStrategy>();
        
        await using (var dbContext = GetInMemoryDbContext())
        {
            var repository = new OwnerRepository(dbContext, fCbf, fStrategy);

            await dbContext.AddAsync(owner);
            await dbContext.SaveChangesAsync();
            
            //Act
            await repository.DeleteAsync(owner.Id, confirmDelete: true);
        }

        //Assert
        await using (var dbContext = GetInMemoryDbContext())
        {
            var markedOwner = await dbContext.PetOwners.FindAsync(owner.Id);
            markedOwner.Should().Be(null);
        }
    }
    
    //TODO: Add the exception path

    [Fact]
    public void AddPetAsync_ShouldInvokePetAddedEvent()
    {
        //Arrange
        var pet = CreatePet();
        var owner = CreateOwner();
        
        var fCbf = A.Fake<CircuitBreaker.CircuitBreakerFactory>();
        var dbContext = GetInMemoryDbContext();
        var fStrategy = A.Fake<IOwnerEventStrategy>();
        
        var repository = new OwnerRepository(dbContext, fCbf, fStrategy);
        
        //Act
        repository.AddPet(owner, pet);
    
        //Assert
        A.CallTo(() => fStrategy.HandlePetAdded(A<Owner>._)).MustHaveHappenedOnceExactly();
        pet.PetOwner?.Id.Should().Be(owner.Id);
    }
    
    [Fact]
    public void RemovePetAsync_ShouldInvokePetRemovedEvent()
    {
        //Arrange
        var pet = CreatePet();
        var owner = CreateOwner();
        
        var fCbf = A.Fake<CircuitBreaker.CircuitBreakerFactory>();
        var dbContext = GetInMemoryDbContext();
        var fStrategy = A.Fake<IOwnerEventStrategy>();
        
        var repository = new OwnerRepository(dbContext, fCbf, fStrategy);
        
        //Act
        repository.RemovePet(owner,pet);
    
        //Assert
        A.CallTo(() => fStrategy.HandlePetRemoved(A<Owner>._)).MustHaveHappenedOnceExactly();
        pet.PetOwner?.Id.Should().Be(null);
    }

    [Fact]
    public void OwnerRepository_ShouldBeUnsubscribedFromEvents()
    {
        throw new NotImplementedException();
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

    private static Owner CreateOwner(Guid id,string name, int series, int number, bool status)
    {
        var fOwner = new Owner
        {
            Id = id,
            Name = name,
            PassportSeries = series,
            PassportNumber = number,
            IsMarkedToDelete = status,
            OwnedPets = new List<Pet>()
        };
        return fOwner;
    }

    private static Pet CreatePet()
    {
        var pet = new Pet
        {
            Id = Guid.NewGuid(),
            Name = "name",
            PetAge = 10,
            IsMarkedToDelete = false
        };
        return pet;
    }
}