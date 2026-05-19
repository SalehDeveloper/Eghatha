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
app.MapHub<TeamHub>(TeamHub.HubUrl);



//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    var dbContext = services.GetRequiredService<AppDbContext>();
//    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
//    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

//    // Prevent duplicate seeding
//    if (await dbContext.Users.AnyAsync())
//    {
//        // already seeded
//    }
//    else
//    {
//        // 1. Create roles
//        var roles = new[] { ApplicationRole.Admin, ApplicationRole.TeamMember, ApplicationRole.Volunteer };
//        foreach (var r in roles)
//        {
//            if (!await roleManager.RoleExistsAsync(r))
//            {
//                await roleManager.CreateAsync(new IdentityRole<Guid>(r));
//            }
//        }

//        // 2. Syrian provinces and representative cities
//        var provinces = new[] {
//            ("Damascus", "Damascus", 33.5138, 36.2765),
//            ("Aleppo", "Aleppo", 36.2021, 37.1343),
//            ("Homs", "Homs", 34.7308, 36.7090),
//            ("Hama", "Hama", 35.1318, 36.7578),
//            ("Latakia", "Latakia", 35.5319, 35.7760),
//            ("Tartus", "Tartus", 34.8896, 35.8860),
//            ("Idlib", "Idlib", 35.9300, 36.6330),
//            ("Deir ez-Zor", "Deir ez-Zor", 35.3333, 40.1333),
//            ("Raqqa", "Raqqa", 35.9500, 39.0167),
//            ("Daraa", "Daraa", 32.6200, 36.1050)
//        };

//        var photoUrls = new[]
//        {
//            "https://randomuser.me/api/portraits/men/11.jpg",
//            "https://randomuser.me/api/portraits/women/12.jpg",
//            "https://images.unsplash.com/photo-1544005313-94ddf0286df2",
//            "https://randomuser.me/api/portraits/men/13.jpg",
//            "https://randomuser.me/api/portraits/women/14.jpg",
//            "https://images.unsplash.com/photo-1524504388940-b1c1722653e1",
//            "https://randomuser.me/api/portraits/men/15.jpg",
//            "https://randomuser.me/api/portraits/women/16.jpg",
//            "https://images.unsplash.com/photo-1531123897727-8f129e1688ce",
//            "https://randomuser.me/api/portraits/men/17.jpg"
//        };

//        var firstNames = new[] { "Ahmad", "Mariam", "Omar", "Lina", "Khaled", "Rana", "Youssef", "Dina", "Samer", "Nadia" };
//        var lastNames = new[] { "Al-Assad", "Al-Hariri", "Al-Khalil", "Al-Masri", "Al-Haddad", "Al-Fayed", "Al-Amin", "Al-Zein", "Al-Saleh", "Al-Rashid" };

//        var pdfs = new[] {
//            "https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf",
//            "https://www.africau.edu/images/default/sample.pdf"
//        };

//        var volunteers = new List<Volunteer>();
//        var registrations = new List<VolunteerRegisteration>();
//        var teams = new List<Team>();

//        // create an admin user to be team creator
//        var adminUser = new ApplicationUser("System", "Admin", "admin@eghatha.local", "+963000000000", photoUrls[0]);
//        await userManager.CreateAsync(adminUser, "Password123!");
//        await userManager.AddToRoleAsync(adminUser, ApplicationRole.Admin);

//        // create 9 more users (make total 10)
//        for (int i = 0; i < 9; i++)
//        {
//            var firstName = firstNames[i % firstNames.Length];
//            var lastName = lastNames[i % lastNames.Length];
//            var email = $"{firstName.ToLower()}.{lastName.ToLower()}{i}@example.com";
//            var phone = $"+9639{i:00000000}";
//            var photo = photoUrls[i % photoUrls.Length];

//            var user = new ApplicationUser(firstName, lastName, email, phone, photo);
//            await userManager.CreateAsync(user, "Password123!");

//            // assign roles: make first 4 team members, remaining volunteers
//            if (i < 4)
//                await userManager.AddToRoleAsync(user, ApplicationRole.TeamMember);
//            else
//                await userManager.AddToRoleAsync(user, ApplicationRole.Volunteer);

//            // create volunteer for every identity user (including admin)
//            var provinceEntry = provinces[(i % provinces.Length)];
//            var lat = provinceEntry.Item3 + (i * 0.001);
//            var lng = provinceEntry.Item4 + (i * 0.001);
//            var location = GeoLocation.Create(lat, lng).Value;

//            var volunteerResult = Volunteer.Create(
//                Guid.NewGuid(),
//                user.Id,
//                VolunteerStatus.Available,
//                VolunteerSpeciality.General,
//                location,
//                provinceEntry.Item1,
//                provinceEntry.Item2,
//                2 + i,
//                pdfs[i % pdfs.Length]
//            );

//            if (!volunteerResult.IsError)
//            {
//                volunteers.Add(volunteerResult.Value);

//                var reg = VolunteerRegisteration.Create(volunteerResult.Value.Id, DateTimeOffset.UtcNow.AddDays(-i));
//                if (!reg.IsError)
//                    registrations.Add(reg.Value);
//            }
//        }

//        // add admin as volunteer as well (created earlier)
//        var adminProvince = provinces[0];
//        var adminLocation = GeoLocation.Create(adminProvince.Item3, adminProvince.Item4).Value;
//        var adminVolunteer = Volunteer.Create(Guid.NewGuid(), adminUser.Id, VolunteerStatus.Available, VolunteerSpeciality.FirstAid, adminLocation, adminProvince.Item1, adminProvince.Item2, 5, pdfs[0]);
//        if (!adminVolunteer.IsError)
//        {
//            volunteers.Add(adminVolunteer.Value);
//            var regAdmin = VolunteerRegisteration.Create(adminVolunteer.Value.Id, DateTimeOffset.UtcNow.AddDays(-1));
//            if (!regAdmin.IsError) registrations.Add(regAdmin.Value);
//        }

//        // Create 10 teams and attach members and resources
//        for (int i = 0; i < 10; i++)
//        {
//            var provinceEntry = provinces[i % provinces.Length];
//            var name = $"Team {i + 1} - {provinceEntry.Item2}";
//            var speciality = TeamSpeciality.List.ElementAt(i % TeamSpeciality.List.Count);
//            var location = GeoLocation.Create(provinceEntry.Item3 + 0.002 * i, provinceEntry.Item4 + 0.002 * i).Value;

//            var teamResult = Team.Create(Guid.NewGuid(), name, speciality, provinceEntry.Item1, provinceEntry.Item2, location, adminUser.Id);
//            if (teamResult.IsError) continue;
//            var team = teamResult.Value;

//            // add up to 3 members from the created volunteers/users
//            var memberUsers = volunteers.Skip(i).Take(3).ToList();
//            var leaderSet = false;
//            foreach (var v in memberUsers)
//            {
//                var isLeader = !leaderSet;
//                var memberResult = team.AddMember(v.UserId, isLeader ? "Leader" : "Member", isLeader, DateTimeOffset.UtcNow.AddDays(-i));
//                if (!memberResult.IsError)
//                {
//                    leaderSet = true;
//                }
//            }

//            // add a resource
//            var resourceType = ResourceType.List.ElementAt(i % ResourceType.List.Count);
//            var res = team.AddResource(5 + i, resourceType);

//            teams.Add(team);
//        }

//        // Create 10 disasters
//        for (int i = 0; i < 10; i++)
//        {
//            var provinceEntry = provinces[i % provinces.Length];
//            var type = DisasterType.List.ElementAt(i % DisasterType.List.Count);
//            var location = GeoLocation.Create(provinceEntry.Item3 + 0.003 * i, provinceEntry.Item4 + 0.003 * i).Value;
//            var reporter = ReporterInfo.Create("Reporter " + (i + 1), $"ID{i + 1000}", $"+9636{i:0000000}").Value;

//            var disasterResult = Disaster.Create(Guid.NewGuid(), type, $"Disaster {i + 1} in {provinceEntry.Item2}", "Sample description", location, provinceEntry.Item1, provinceEntry.Item2, DateTimeOffset.UtcNow.AddHours(-i * 3), reporter, null);
//            if (disasterResult.IsError) continue;

//            // assign one team if available
//            var teamToAssign = teams.ElementAtOrDefault(i % teams.Count);
//            if (teamToAssign != null)
//            {
//                disasterResult.Value.AssignTeam(teamToAssign.Id);
//            }
//        }

//        // persist
//        dbContext.Set<Volunteer>().AddRange(volunteers);
//        dbContext.Set<VolunteerRegisteration>().AddRange(registrations);
//        dbContext.Set<Team>().AddRange(teams);

//        await dbContext.CompleteAsync(CancellationToken.None);
//    }
//}

app.Run();