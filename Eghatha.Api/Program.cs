using Eghatha.Api;
using Eghatha.Application;
using Eghatha.Infastructure;


var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services
    .AddPresentation(config)
    .AddApplication()
    .AddInfrastructure(config);

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapApiDocs();
else
    app.UseHsts();



app.UseCoreMiddlewares(config);
app.MapControllers();
app.MapHubs();
app.Run();