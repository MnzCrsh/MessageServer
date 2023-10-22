using MessageServer;

var builder = WebApplication.CreateBuilder(args);


builder.Services.RunServices();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
