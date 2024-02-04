using MessageServer.Application;

var builder = WebApplication.CreateBuilder(args);


builder.RunPostgresDb();
builder.RunServices();


var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
