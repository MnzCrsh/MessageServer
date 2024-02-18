using MessageServer.Domain;

namespace MessageServer.Infrastructure.Repositories.Implementations;

public class OwnerEventArgs : EventArgs
{
    public OwnerEventArgs(Owner? owner)
    {
        Owner = owner;
    }

    public Owner? Owner { get; }
}