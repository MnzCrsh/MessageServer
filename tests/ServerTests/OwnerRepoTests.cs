﻿using FakeItEasy;
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

    [Fact]
    public async Task GetAsync_Should_Return_Owner_By_Valid_Id()
    {
        //Arrange
        // var ownerId = Guid.Empty;
        var expectedOwner = CreateOwner();
        
        var fCbf = A.Fake<CircuitBreaker.CircuitBreakerFactory>();

        await using var dbContext = GetInMemoryDbContext();
        
        dbContext.Add(expectedOwner);
        await dbContext.SaveChangesAsync();

        var repository = new OwnerRepository(dbContext, fCbf);
        
        //Act
        var result = await repository.GetAsync(expectedOwner.Id);
        
        //Assert
        result.Id.Should().Be(expectedOwner.Id);
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
            Id = Guid.Empty,
            Name = "name",
            PassportSeries = 0123,
            PassportNumber = 456789,
            IsMarkedToDelete = false,
            OwnedPets = null
        };
        return fOwner;
    }
}