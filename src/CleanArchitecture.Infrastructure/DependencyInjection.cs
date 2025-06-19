using Azure.Storage.Blobs;
using CleanArchitecture.Application.ServiceContracts;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.RepositoryContracts;
using CleanArchitecture.Domain.RepositoryContracts.Base;
using CleanArchitecture.Domain.RepositoryContracts.UnitOfWork;
using CleanArchitecture.Infrastructure.Auth;
using CleanArchitecture.Infrastructure.AzureBlobService;
using CleanArchitecture.Infrastructure.Data.Interceptors;
using CleanArchitecture.Infrastructure.Redis;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.Infrastructure.Repositories.UnitOfWork;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
      IConfiguration configuration)
  {
    // Add services to the container
    services.AddIdentityServer()
        //.AddAspNetIdentity<User>()
        .AddInMemoryApiScopes(Config.ApiScopes) // Define API scopes
        .AddInMemoryApiResources(Config.ApiResources) // Define API resources
        .AddInMemoryClients(Config.Clients) // Define clients
        .AddInMemoryIdentityResources(Config.IdentityResources)
        .AddDeveloperSigningCredential() // Use for dev, use a real certificate in prod
        .AddProfileService<ProfileService>();

    services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

    services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
    {
      options.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
      options.EnableSensitiveDataLogging();

      string? connectionString = Environment.GetEnvironmentVariable("databaseConnectionString");
      if (string.IsNullOrEmpty(connectionString))
        connectionString = configuration.GetConnectionString("DevDatabase");
       //options.UseInMemoryDatabase("database");
      options.UseNpgsql(connectionString);
    });
    services.AddHttpClient("Gemini");
    // GUIDE: Put azure.env file in the CleanArchitecture.Presentation directory
    Env.Load("azure.env");
    string? azureBlobConnectionString = Environment.GetEnvironmentVariable("azureBlobConnectionString");
    if (string.IsNullOrEmpty(azureBlobConnectionString))
    {
      azureBlobConnectionString = configuration["AzureBlobConnectionString"];
    }
    services.AddSingleton(x => new BlobServiceClient(azureBlobConnectionString));
    services.AddSingleton<IBlobService, BlobService>();
    
    services.AddStackExchangeRedisCache(options =>
    {
      options.Configuration = configuration.GetConnectionString("Redis");
    });

    services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

    // Register Unit of Work
    services.AddScoped<IUnitOfWork, UnitOfWork>();

    // Register Generic Repository
    services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

    #region Register Repositories

    services.AddScoped<IBatchRepository, BatchRepository>();
    services.AddScoped<IBlogRepository, BlogRepository>();
    services.AddScoped<IBlogTagRepository, BlogTagRepository>();
    services.AddScoped<IBrandRepository, BrandRepository>();
    services.AddScoped<ICartItemRepository, CartItemRepository>();
    services.AddScoped<ICartRepository, CartRepository>();
    services.AddScoped<ICategoryRepository, CategoryRepository>();
    services.AddScoped<ICompanyInformationRepository, CompanyInformationRepository>();
    services.AddScoped<ICosmeticImageRepository, CosmeticImageRepository>();
    services.AddScoped<ICosmeticRepository, CosmeticRepository>();
    services.AddScoped<ICosmeticSubCategoryRepository, CosmeticSubCategoryRepository>();
    services.AddScoped<ICosmeticTypeRepository, CosmeticTypeRepository>();
    services.AddScoped<ICouponRepository, CouponRepository>();
    services.AddScoped<IFAQRepository, FAQRepository>();
    services.AddScoped<IFeedbackRepository, FeedbackRepository>();
    services.AddScoped<IOrderItemRepository, OrderItemRepository>();
    services.AddScoped<IOrderRepository, OrderRepository>();
    services.AddScoped<IPaymentRepository, PaymentRepository>();
    services.AddScoped<IPolicyRepository, PolicyRepository>();
    services.AddScoped<IQuestionOptionRepository, QuestionOptionRepository>();
    services.AddScoped<IQuestionRepository, QuestionRepository>();
    services.AddScoped<IQuestionTypeRepository, QuestionTypeRepository>();
    services.AddScoped<IQuizRepository, QuizRepository>();
    services.AddScoped<IRefundRepository, RefundRepository>();
    services.AddScoped<IRefundItemRepository, RefundItemRepository>();
    services.AddScoped<IRoutineRepository, RoutineRepository>();
    services.AddScoped<IRoutineStepRepository, RoutineStepRepository>();
    services.AddScoped<ISkinTypeRepository, SkinTypeRepository>();
    services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
    services.AddScoped<ITagRepository, TagRepository>();
    services.AddScoped<ITestimonialRepository, TestimonialRepository>();
    services.AddScoped<IEventRepository, EventRepository>();
    services.AddScoped<ICosmeticPriceRepository, CosmeticPriceRepository>();
    #endregion

    // Register Redis Caching
    services.AddScoped<IRedisCacheRepository, RedisCacheRepository>();

    return services;
  }
}
