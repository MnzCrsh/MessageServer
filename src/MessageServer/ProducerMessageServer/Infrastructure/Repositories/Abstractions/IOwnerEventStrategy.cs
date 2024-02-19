using MessageServer.Domain;

namespace MessageServer.Infrastructure.Repositories.Abstractions;

public interface IOwnerEventStrategy
{
    Task HandlePetAdded(Owner owner);
    Task HandlePetRemoved(Owner owner);
}