using Autofac;
using MessageServer.Infrastructure.Repositories.Implementations;
using MessageServer.Infrastructure.Repositories.Interfaces;

namespace MessageServer.Application;

public class DiContainerModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<OwnerRepository>().As<IOwnerRepository>().InstancePerLifetimeScope();
        builder.RegisterType<PetRepository>().As<IPetRepository>().InstancePerLifetimeScope();
    }
}