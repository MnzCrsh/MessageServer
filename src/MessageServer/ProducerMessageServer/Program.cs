using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.WebApi;
using MessageServer.Application;

var builder = WebApplication.CreateBuilder(args);


builder.RunPostgresDb();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new DiContainerModule());
    containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());
} );

builder.RunServiceConfiguration();


var app = builder.Build();


app.UseHsts();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
