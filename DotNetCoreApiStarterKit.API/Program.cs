using DotNetCoreApiStarterKit.API.Extensions;
using DotNetCoreApiStarterKit.API.Middleware;
using DotNetCoreApiStarterKit.Domain.Entities.Identity;
using DotNetCoreApiStarterKit.Infrastructure.DbContext;
using DotNetCoreApiStarterKit.Infrastructure.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// DATABASE CONFIGURATION
// ==========================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==========================================
// IDENTITY CONFIGURATION
// ==========================================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = true;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ==========================================
// JWT AUTHENTICATION CONFIGURATION
// ==========================================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
.AddJwtBearer("JwtBearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)) // Ensure key length >= 256 bits
    };
});

// ==========================================
// GLOBAL CORS POLICY
// ==========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ==========================================
// CONTROLLERS, DEPENDENCIES, SERVICES
// ==========================================
builder.Services.AddControllers();
builder.Services.AddApplicationServices(); // Custom extension for services
builder.Services.AddOpenApi();            // Custom extension if you defined OpenAPI configs
builder.Services.AddSwaggerGen();         // Swagger generator

// ==========================================
// SWAGGER CONFIG WITH JWT AUTH SUPPORT
// ==========================================
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DotNetCore API Starter Kit",
        Version = "v1",
        Description = "Starter API with EF Core, Identity, and JWT"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {your JWT token}'"
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ==========================================
// SEED DEFAULT USERS AND ROLES
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await DefaultDataSeeder.SeedDefaultDataAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Seeding error: {ex.Message}");
    }
}

// ==========================================
// MIDDLEWARE PIPELINE
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");                  // Enable CORS globally
app.UseMiddleware<ExceptionMiddleware>(); // Custom global exception handler
app.UseAuthentication();                  // Enable authentication middleware
app.UseAuthorization();                   // Enable authorization middleware

app.MapControllers();                     // Map all controller endpoints

app.Run();
