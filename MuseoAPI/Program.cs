using MuseoData.Repositories;
using MuseoAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MuseoAuth;
using Adapter;
using Adapter.Settings;
using Adapter.Storage;
using MuseoShared.Interfaces;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
{
    ["ConnectionStrings:DefaultConnection"] =
        Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"),

    ["Jwt:Key"] =
        Environment.GetEnvironmentVariable("Jwt__Key"),

    ["Jwt:Issuer"] =
        Environment.GetEnvironmentVariable("Jwt__Issuer"),

    ["Jwt:Audience"] =
        Environment.GetEnvironmentVariable("Jwt__Audience"),

    ["Jwt:ExpirationHours"] =
        Environment.GetEnvironmentVariable("JWT_EXPIRATION_HOURS")
});

builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<AuthenticationService>();

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<DbConnectionFactory>();

builder.Services.AddScoped<ExhibitRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<MuseumRepository>();
builder.Services.AddScoped<AnnouncementRepository>();
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<MediaRepository>();
builder.Services.AddScoped<PasswordService>();

builder.Services.Configure<MinioSettings>(
    builder.Configuration.GetSection("Minio"));

builder.Services.AddScoped<IStorageService, MinioStorageService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();