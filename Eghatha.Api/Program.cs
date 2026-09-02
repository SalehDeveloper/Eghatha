using Eghatha.Api;
using Eghatha.Application;
using Eghatha.Domain.Resources;
using Eghatha.Infastructure;
using Eghatha.Infastructure.RealTime.Admin;
using Eghatha.Infastructure.RealTime.Team;
using FluentValidation;
using Scalar.AspNetCore;
using System.Globalization;

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
                          policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500", "https://localhost:7243")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();

                      });


});




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
//app.UseRequestLocalization();


app.Use(async (context, next) =>
{
    var lang = context.Request.Headers.AcceptLanguage;
    //["Accept-Language"].ToString()

    if (!string.IsNullOrEmpty(lang))
    {
        //var culture = new System.Globalization.CultureInfo(lang);
        var language = "ar";

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(language);

        Thread.CurrentThread.CurrentCulture = new CultureInfo(language);

        Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo(language);



    }

    await next();
});

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

//TODO: Add middleware to handle selected language from request header






app.UseCoreMiddlewares(config);
app.MapControllers();

app.Map("/lang1", () => Results.Ok(TeamErrorsTest.TeamErrors_Speciality_Invalid));

app.Map("/lang2", () => Results.Ok(VolunteerErrors.VolunteerErrors_SpecialityInvalid));

app.MapHub<AdminHub>(AdminHub.HubUrl);
app.MapHub<TeamHub>(TeamHub.HubUrl);




app.Run();