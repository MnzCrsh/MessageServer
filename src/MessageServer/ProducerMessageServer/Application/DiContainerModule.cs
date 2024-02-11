﻿using Autofac;
using MessageServer.Infrastructure;
using MessageServer.Infrastructure.Repositories.Implementations;
using MessageServer.Infrastructure.Repositories.Interfaces;

namespace MessageServer.Application;

public class DiContainerModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<OwnerRepository>().As<IOwnerRepository>().InstancePerLifetimeScope();
        builder.RegisterType<PetRepository>().As<IPetRepository>().InstancePerLifetimeScope();

        
        //TODO: Add comments
        builder.RegisterType<CircuitBreaker>().AsSelf();

        builder.Register<Func<TimeSpan, CircuitBreaker>>(c =>
        {
            var context = c.Resolve<IComponentContext>();
            
            return timeout => context.Resolve<CircuitBreaker>
                (new TypedParameter(typeof(TimeSpan), timeout));
        });
    }
}