

using Eghatha.Application;
using Eghatha.Infastructure;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddApplication().AddInfrastructure(config);

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();