using Eghatha.Api.Services;
using Eghatha.Application.Common.Authentication;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Common.Services;
using Eghatha.Domain.Abstractions;
using Eghatha.Infastructure.Data;
using Eghatha.Infastructure.Data.Interceptors;
using Eghatha.Infastructure.Identity;
using Eghatha.Infastructure.Identity.Policies;
using Eghatha.Infastructure.RealTime;
using Eghatha.Infastructure.Repositories;
using Eghatha.Infastructure.Services;
using Eghatha.Infastructure.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using static Eghatha.Infastructure.Services.EmailService;





namespace Eghatha.Infastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHybridCache(options => options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(10), // L2, L3
                LocalCacheExpiration = TimeSpan.FromSeconds(30), // L1
            });
            AddPersistence(services, configuration);
            AddAuthentication(services, configuration);

            return services;
        }

        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(TimeProvider.System);

            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

            services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Database"));

                options.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());

              
                options.EnableSensitiveDataLogging();
            });

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

            services
                .AddIdentityCore<ApplicationUser>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredUniqueChars = 1;
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedEmail = true;
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.User.RequireUniqueEmail = true;
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddHttpContextAccessor();
            services.AddScoped<IUser, CurrentUser>();

           services.Configure<OtpSettings>(
                                 configuration.GetSection("OtpSettings"));

            services.Configure<EmailOptions>(configuration.GetSection("EmailConfiguration"));
            services.Configure<CloudinaryOptions>(configuration.GetSection("Cloudinary"));

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                
                return ConnectionMultiplexer.Connect(configuration["Redis:Connection"]);
            });

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IRefreshTokenRepository , RefreshTokenRepository>();
            services.AddScoped<IRedisCacheService, RedisCacheService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IOtpCodeGenerator, OtpCodeGenerator>();
            services.AddSingleton<IEmailTemplateBuilder, EmailTemplateBuilder>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IAdminNotifier , SignalRAdminNotifier>();
            services.AddScoped<ITeamLocationService, TeamLocationService>();
            services.AddScoped<IVolunteerRegisterationRepository, VolunteerRegisterationRepository>();
            services.AddScoped<IVolunteerRepository, VolunteerRepository>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();


            services.AddHttpClient<IGeocodingService, OpenStreetMapService>(client =>
            {
                client.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");

                client.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "VolunteerSystem/1.0 (ahmadsalehdev.22@gmail.com)"
                );

                client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            });
        }


        private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Eghatha.Infastructure.Identity.AuthenticationOptions>(
                     configuration.GetSection("Authentication"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer();

            services.ConfigureOptions<JwtBearerOptionsSetup>();
            services.AddScoped<IJwtService, JwtService>();

            services.AddTransient<IIdentityService, IdentityService>();
            services.AddScoped<ICookieService , CookieService>();
            services.AddScoped<IAuthorizationHandler, MustBeTeamLeaderHandler>();

            services.AddAuthorizationBuilder()
                .AddPolicy("CanUpdateTeamLocation", policy =>
                {
                    policy.Requirements.Add(new MustBeTeamLeaderRequirment());
                });
        }

    }
}
