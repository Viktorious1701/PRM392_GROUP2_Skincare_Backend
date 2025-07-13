// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Presentation/DependencyInjection.cs
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Presentation.Middlewares;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Reflection;
using System.Text;

namespace CleanArchitecture.Presentation;

public static class DependencyInjection
{
  public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
  {
    var fullVersion = Assembly.GetExecutingAssembly()
                          .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                          .InformationalVersion ?? "1.0.0";
    var version = fullVersion.Split('+')[0]; // Remove commit hash if present

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(options =>
    {
      options.SwaggerDoc("v1", new OpenApiInfo
      {
        Title = $"De Fleur API - {version}",
        Version = version
      });
      options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
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
          new string[]{}
        }
      });
    });

    // Add identity
    services.AddIdentity<User, Role>(options =>
    {
      options.Password.RequiredLength = 5;
      options.Password.RequireNonAlphanumeric = false;
      options.Password.RequireUppercase = false;
      options.Password.RequireLowercase = false;
      options.Password.RequireDigit = false;
      options.Password.RequiredUniqueChars = 0;

      options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
      options.Lockout.MaxFailedAccessAttempts = 5;
      options.Lockout.AllowedForNewUsers = true;
    })
      .AddEntityFrameworkStores<ApplicationDbContext>()
      .AddDefaultTokenProviders()
      .AddUserStore<UserStore<User, Role, ApplicationDbContext, Guid>>()
      .AddRoleStore<RoleStore<Role, ApplicationDbContext, Guid>>();

    services.AddCors(options =>
    {
      options.AddPolicy("AllowAll",
          policy => policy
              .AllowAnyOrigin()   // ✅ Allow any frontend
              .AllowAnyMethod()   // ✅ Allow GET, POST, PUT, DELETE, etc.
              .AllowAnyHeader()); // ✅ Allow any headers
    });

    // Add authentication & authorization
    services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
      .AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = config["JwtSettings:Issuer"],
          ValidAudience = config["JwtSettings:Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!))
        };
      });

    services.AddAuthorization(options =>
    {
      options.AddPolicy("ClientIdPolicy", policy => policy.RequireClaim("client_id", "client"));
    });

    services.AddCarter();

    services.AddHealthChecks().AddNpgSql(config.GetConnectionString("DevDatabase")!);

    return services;
  }

  public static WebApplication UseApiServices(this WebApplication app)
  {
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger(options =>
      {
        //https://0.0.0.0:5051/scalar/
        options.RouteTemplate = "/openapi/{documentName}.json";
      });
      app.MapScalarApiReference(options =>
      {
        options.WithTheme(ScalarTheme.Solarized)
            .WithDarkMode(true)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
            .WithDarkModeToggle(false)
            .WithPreferredScheme("Bearer")
            .WithHttpBearerAuthentication(bearer =>
            {
              bearer.Token = "your-bearer-token";
            });
        options.Authentication = new ScalarAuthenticationOptions
        {
          PreferredSecurityScheme = JwtBearerDefaults.AuthenticationScheme,
        };
      });
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/openapi/v1.json", "De Fleur API");
        //c.RoutePrefix = string.Empty;
      });
    }

    app.UseExceptionHandler(options => { });
    app.UseMiddleware<CustomErrorHandler>();

    // FIX: Do not force HTTPS redirection in the development environment.
    // This allows the Android emulator's WebView to communicate with the local backend
    // over HTTP without encountering SSL certificate errors.
    if (!app.Environment.IsDevelopment())
    {
      app.UseHttpsRedirection();
    }

    app.UseRouting();
    // IdentityServer is not needed if AuthService handles tokens, but keep for other potential uses.
    app.UseIdentityServer();
    app.UseCors("AllowAll"); // ✅ Apply CORS globally
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapCarter();

    app.UseHealthChecks("/health",
      new HealthCheckOptions
      {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
      });

    return app;
  }
}