using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Data.Common;
using TrueVote.Database;
using TrueVote.Jobs;
using TrueVote.Models;
using TrueVote.Services;
using TrueVote.Utilities;

var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.UseUrls("http://0.0.0.0:80");

// Add services to the container.
ConfigureServices(builder.Services);

var app = builder.Build();

try
{
    // Migrate database
    MigrateDatabase(app);

    // Configure middleware
    ConfigureMiddleware(app);

    app.Run();
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while starting the application.");
    throw;
}

void ConfigureServices(IServiceCollection services)
{
    // Endpoints
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddHttpContextAccessor();

    // Swagger
    services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    });

    // DB context
    services.AddDbContext<VotingRecordsContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging());

    // Identity Config
    services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<VotingRecordsContext>()
        .AddDefaultTokenProviders();


    services.AddTransient<CsvService>();
    services.AddTransient<DbService>();
    services.AddTransient<RoleService>();
    services.AddScoped<CsvUploader>();


    // Hangfire
    services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));
    services.AddHangfireServer();
}

void MigrateDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<VotingRecordsContext>();

        context.Database.Migrate();
        //CreateStoredProcedures(context.Database.GetDbConnection(), app.Logger);

        // Create roles and administrator user
        var roleService = services.GetRequiredService<RoleService>();
        roleService.CreateRolesAsync().Wait();
        roleService.CreateAdminUserAsync().Wait();
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while migrating the database.");
        throw;
    }
}

void CreateStoredProcedures(DbConnection connection, ILogger logger)
{
    try
    {
        var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "Scripts", "StoredProcedures", "procedures.sql");
        var sqlScript = File.ReadAllText(scriptPath);

        using (var command = connection.CreateCommand())
        {
            command.CommandText = sqlScript;
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        logger.LogInformation("Stored procedure created/updated successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while creating/updating the stored procedure.");
        throw;
    }
}

void ConfigureMiddleware(WebApplication app)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();

    try
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        var cacheMaxAgeOneWeek = (60 * 60 * 24 * 7).ToString();
        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = ctx =>
            {
                ctx.Context.Response.Headers.Append(
                     "Cache-Control", $"public, max-age={cacheMaxAgeOneWeek}");
            }
        });

        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = [new AllowAllAuthorizationFilter()]
        });

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var csvUploader = services.GetRequiredService<CsvUploader>();

            RecurringJob.AddOrUpdate(
                "csv-processing",
                () => csvUploader.processFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "RESULTADOS_2024_CSV_V2-short.csv")),
                Cron.Minutely()
            );
        }

        //app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while configuring middleware.");
        throw;
    }
}
