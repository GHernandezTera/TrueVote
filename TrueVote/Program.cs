using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TrueVote.Database;
using TrueVote.Jobs;
using TrueVote.Models;
using TrueVote.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:80");

// Add services to the container.
ConfigureServices(builder.Services);

var app = builder.Build();

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

app.UseAuthorization();

app.UseHangfireDashboard();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var csvUploader = services.GetRequiredService<CsvUploader>();

    RecurringJob.AddOrUpdate(
        "csv-processing", 
        () => csvUploader.processFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "RESULTADOS_2024_CSV_V2.csv")),
        Cron.Minutely()
    );
}

app.MapControllers();

app.Run();

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
    services.AddScoped<CsvUploader>();


    // Hangfire
    services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));
    services.AddHangfireServer();
}
