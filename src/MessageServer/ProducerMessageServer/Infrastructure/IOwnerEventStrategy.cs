using MessageServer.Domain;

namespace MessageServer.Infrastructure;

public interface IOwnerEventStrategy
{
    Task HandlePetAdded(Owner owner);
    Task HandlePetRemoved(Owner owner);
}