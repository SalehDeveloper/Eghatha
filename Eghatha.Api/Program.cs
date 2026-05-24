using Bogus;
using Eghatha.Api;
using Eghatha.Application;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.Disasters;
using System.Linq;
using Eghatha.Domain.Teams;
using Eghatha.Domain.Teams.Resources;
using Eghatha.Domain.Teams.TeamMembers;
using Eghatha.Domain.VolunteerRegisterations;
using Eghatha.Domain.Volunteers;
using Eghatha.Domain.Volunteers.Equipments;
using Eghatha.Infastructure;
using Eghatha.Infastructure.Data;
using Eghatha.Infastructure.RealTime.Admin;
using Eghatha.Infastructure.RealTime.Team;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using StackExchange.Redis;

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
                          policy.WithOrigins("http://127.0.0.1:5500" , "http://localhost:5500")
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
app.MapHub<TeamHub>(TeamHub.HubUrl);





app.Run();