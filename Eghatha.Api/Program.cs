using Bogus;
using Eghatha.Api;
using Eghatha.Application;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Shared.ValueObjects;
using Eghatha.Domain.Teams;
using Eghatha.Domain.Teams.Resources;
using Eghatha.Domain.Teams.TeamMembers;
using Eghatha.Domain.VolunteerRegisterations;
using Eghatha.Domain.Volunteers;
using Eghatha.Domain.Volunteers.Equipments;
using Eghatha.Infastructure;
using Eghatha.Infastructure.Data;
using Eghatha.Infastructure.RealTime;
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
                          policy.WithOrigins("http://localhost:5250" , "http://localhost:5500" , "http://localhost:8080")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials()
                                ;
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



//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    var dbContext = services.GetRequiredService<AppDbContext>();
//    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

//    // 🔁 Prevent duplicate seeding
  

//    var random = new Random();

//    var firstNames = new[] { "John", "Jane", "Michael", "Sara", "David", "Emma", "Ali", "Omar", "Lina", "Noah" };
//    var lastNames = new[] { "Smith", "Johnson", "Brown", "Taylor", "Anderson", "Thomas", "Moore", "Martin", "Lee", "Walker" };

//    var pdfs = new[]
//    {
//        "https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf",
//        "https://www.africau.edu/images/default/sample.pdf"
//    };

//    var specialities = new[]
//    {
//        VolunteerSpeciality.Medical,
//        VolunteerSpeciality.SearchAndRescue,
//        VolunteerSpeciality.FireFighting,
//        VolunteerSpeciality.Logistics,
//        VolunteerSpeciality.Engineering
//    };

//    var volunteers = new List<Volunteer>();
//    var registrations = new List<VolunteerRegisteration>();

//    for (int i = 0; i < 10; i++)
//    {
//        // =========================
//        // 1. User
//        // =========================
//        var firstName = firstNames[i % firstNames.Length];
//        var lastName = lastNames[i % lastNames.Length];

//        var email = $"{firstName.ToLower()}.{lastName.ToLower()}{i}@test.com";

//        var photo = i % 2 == 0
//            ? $"https://randomuser.me/api/portraits/men/{i + 1}.jpg"
//            : $"https://randomuser.me/api/portraits/women/{i + 1}.jpg";

//        var user = new ApplicationUser(
//            firstName,
//            lastName,
//            email,
//            $"12345678{i}",
//            photo
//        );

//        await userManager.CreateAsync(user, "Password123!");

//        // =========================
//        // 2. Volunteer
//        // =========================
//        var lat = random.NextDouble() * 180 - 90;
//        var lng = random.NextDouble() * 360 - 180;

//        var location = GeoLocation.Create(lat, lng).Value;

//        var volunteerResult = Volunteer.Create(
//            Guid.NewGuid(),
//            user.Id,
//            VolunteerStatus.UnAvailable,
//            specialities[random.Next(specialities.Length)],
//            location,
//            random.Next(1, 10),
//            pdfs[random.Next(pdfs.Length)],
//            new List<Equipment>()
//        );

//        if (volunteerResult.IsError)
//            continue;

//        var volunteer = volunteerResult.Value;
//        volunteers.Add(volunteer);

//        // =========================
//        // 3. Registration (ALL PENDING)
//        // =========================
//        var regResult = VolunteerRegisteration.Create(
//            volunteer.Id,
//            DateTimeOffset.UtcNow.AddDays(-random.Next(1, 10))
//        );

//        if (regResult.IsError)
//            continue;

//        registrations.Add(regResult.Value);
//    }

//    dbContext.Set<Volunteer>().AddRange(volunteers);
//    dbContext.Set<VolunteerRegisteration>().AddRange(registrations);

//    await dbContext.SaveChangesAsync();
//}

app.Run();