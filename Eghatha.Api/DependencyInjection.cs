using Eghatha.Api.Infrastructure;
using Eghatha.Api.Middlewares;
using Eghatha.Infastructure.RealTime.Admin;
using Eghatha.Infastructure.RealTime.Team;
using Microsoft.AspNetCore.RateLimiting;
using Scalar.AspNetCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;


namespace Eghatha.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCustomProblemDetails()
                .AddOpenApi()
               .AddCustomApiVersioning()
               .AddExceptionHandling()
               .AddControllerWithJsonConfiguration()
               .AddAppRateLimiting()
               .AddAppOutputCaching()
               .AddSignalR();

            return services;


        }

        public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
        {
            services.AddProblemDetails(options => options.CustomizeProblemDetails = (context) =>
            {
                context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                context.ProblemDetails.Extensions.Add("requestId", context.HttpContext.TraceIdentifier);
            });

            return services;
        }

        public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            return services;
        }

        public static IServiceCollection AddControllerWithJsonConfiguration(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            return services;
        }

        public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
        {
            // Use Asp.Versioning.Mvc for .NET 9
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new Asp.Versioning.UrlSegmentApiVersionReader();
            }).AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        public static IServiceCollection AddAppOutputCaching(this IServiceCollection services)
        {
            services.AddOutputCache(options =>
            {
                options.SizeLimit = 100 * 1024 * 1024; // 100 mb
                options.AddBasePolicy(policy =>
                    policy.Expire(TimeSpan.FromSeconds(60)));
            });

            return services;
        }

        public static IServiceCollection AddAppRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddSlidingWindowLimiter("SlidingWindow", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 100;
                    limiterOptions.Window = TimeSpan.FromMinutes(1);
                    limiterOptions.SegmentsPerWindow = 6;
                    limiterOptions.QueueLimit = 10;
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.AutoReplenishment = true;
                });

                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });

            return services;
        }

        public static IServiceCollection AddCors(this IServiceCollection services)
        {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("https://localhost:7243")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
                                            .AllowCredentials();

                                  });
            });

            return services;
        }

        public static IServiceCollection AddOpenApi(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
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

            return services;
        }
        public static WebApplication MapApiDocs(this WebApplication app)
        {
            app.MapOpenApi();

            app.MapScalarApiReference(options =>
            {
                options.WithTitle("Eghatha API")
                       .WithTheme(ScalarTheme.BluePlanet)
                       .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });

            return app;
        }

        public static IApplicationBuilder UseCoreMiddlewares(this IApplicationBuilder app, IConfiguration configuration)
        {

            app.UseMiddleware<LocalizationMiddleware>();

            app.UseExceptionHandler();

            app.UseCors("_myAllowSpecificOrigins");

            app.UseStatusCodePages();

            app.UseHttpsRedirection();

            app.UseRateLimiter();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseOutputCache();

            return app;
        }

        public static IEndpointRouteBuilder MapHubs(this IEndpointRouteBuilder app)
        {
            app.MapHub<AdminHub>(AdminHub.HubUrl);
            app.MapHub<TeamHub>(TeamHub.HubUrl);

            return app;
        }
    }
}