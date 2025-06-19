using CleanArchitecture.Application;
using CleanArchitecture.Application.Mapper;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Data.Extensions;
using CleanArchitecture.Presentation;
using Mapster;
using MapsterMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
  .AddApplicationServices(builder.Configuration)
  .AddInfrastructureServices(builder.Configuration)
  .AddApiServices(builder.Configuration);

// Add all mapping configurations from the assembly where they are defined.
// This is more maintainable than adding each one manually.
var config = TypeAdapterConfig.GlobalSettings;
config.Scan(typeof(CosmeticMappingConfig).Assembly); // This will now scan and find StoreMappingConfig as well.

builder.Services.AddSingleton(config);
builder.Services.AddMapster();
builder.Services.AddScoped<IMapper, ServiceMapper>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  await app.InitializeDatabaseAsync();
}

app.UseApiServices();

app.Run();