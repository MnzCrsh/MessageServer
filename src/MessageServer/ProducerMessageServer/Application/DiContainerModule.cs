using Autofac;
using MessageServer.Infrastructure;
using MessageServer.Infrastructure.Repositories.Abstractions;
using MessageServer.Infrastructure.Repositories.Implementations;

namespace MessageServer.Application;

public class DiContainerModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<OwnerRepository>().As<IOwnerRepository>().InstancePerLifetimeScope();
        builder.RegisterType<PetRepository>().As<IPetRepository>().InstancePerLifetimeScope();
        builder.RegisterType<OwnerEventStrategy>().As<IOwnerEventStrategy>().InstancePerLifetimeScope();

        
        //Delegate factory
        builder.RegisterType<CircuitBreaker>().AsSelf();

        builder.Register<Func<TimeSpan, CircuitBreaker>>(c =>
        {
            var context = c.Resolve<IComponentContext>();
            
            return timeout => context.Resolve<CircuitBreaker>
                (new TypedParameter(typeof(TimeSpan), timeout));
        });
    }
}