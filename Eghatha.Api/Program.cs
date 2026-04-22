using Eghatha.Api;
using Eghatha.Application;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.Teams;
using Eghatha.Infastructure;
using Eghatha.Infastructure.Data;
using Eghatha.Infastructure.RealTime;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using StackExchange.Redis;
using Bogus;
using Eghatha.Domain.Teams.Resources;
using Eghatha.Domain.Teams.TeamMembers;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services
    .AddPresentation(config)
    .AddApplication()
    .AddInfrastructure(config);

builder.Services.AddControllers();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5250")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                      });


});



// Add OpenAPI services for .NET 9
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Servers.Clear();
        document.Servers.Add(new()
        {
            Url = "/"
        });

        return Task.CompletedTask;
    });
});

var app = builder.Build();


app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

if (app.Environment.IsDevelopment())
{
    // Map OpenAPI endpoint
    app.MapOpenApi();

    // Map Scalar API reference
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Eghatha API")
               .WithTheme(ScalarTheme.BluePlanet)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}
else
{
    app.UseHsts();
}



app.UseCoreMiddlewares(config);
app.MapControllers();
app.MapHub<AdminHub>(AdminHub.HubUrl);

app.Run();