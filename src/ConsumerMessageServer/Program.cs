using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.WebApi;
using ConsumerMessageServer;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new DiContainerModule());
    containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());
} );

builder.RunServiceConfiguration();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();



app.Run();

