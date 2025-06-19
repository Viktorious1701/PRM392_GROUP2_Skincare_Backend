using CleanArchitecture.Application.DTOs.Email;
using CleanArchitecture.Application.Factories.FilePathFactory;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Application.Strategies;
using CleanArchitecture.Application.Strategies.BlogFilterStrategy;
using CleanArchitecture.Application.Strategies.CosmeticsFilterStrategy;
using CleanArchitecture.Application.Strategies.InvoiceGenerateStrategy;
using CleanArchitecture.Application.Strategies.ReportGenerateStrategy.ReportTypeStrategy;
using CleanArchitecture.Application.Validators;
using CleanArchitecture.Application.Validators.Blog;
using IdentityServer4.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using QuestPDF.Infrastructure;

namespace CleanArchitecture.Application;

public static class DependencyInjection
{
  public static IServiceCollection AddApplicationServices
    (this IServiceCollection services, IConfiguration configuration)
  {

    QuestPDF.Settings.License = LicenseType.Community;

    services.AddFeatureManagement();
    services.AddHttpContextAccessor();

    // Add all validators
    services.AddValidatorsFromAssemblyContaining<CreateBlogRequestValidator>();

    // Add identity server 4 validator for owner password
    services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();

    // Add Email Configuration
    services.AddSingleton(configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>()!);

    // Add services
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IBatchService, BatchService>();
    services.AddScoped<IBlogService, BlogService>();
    services.AddScoped<IBrandService, BrandService>();
    services.AddScoped<ICartService, CartService>();
    services.AddScoped<ICategoryService, CategoryService>();
    services.AddScoped<ICosmeticService, CosmeticService>();
    services.AddScoped<ICosmeticImageService, CosmeticImageService>();
    services.AddScoped<ICosmeticTypeService, CosmeticTypeService>();
    services.AddScoped<ICouponService, CouponService>();
    services.AddScoped<IEmailService, EmailService>();
    services.AddScoped<IFeedbackService, FeedbackService>();
    services.AddScoped<IOrderService, OrderService>();
    services.AddScoped<IPaymentService, PaymentService>();
    services.AddScoped<IQuizService, QuizService>();
    services.AddScoped<IQuizResultService, QuizResultService>();
    services.AddScoped<IRefundService, RefundService>();
    services.AddScoped<IRefundItemService, RefundItemService>();
    services.AddScoped<ISubCategoryService, SubCategoryService>();
    services.AddScoped<ISkinTypeService, SkinTypeService>();
    services.AddScoped<ITestimonialService, TestimonialService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IRoleService, RoleService>();
    services.AddScoped<IRoutineService, RoutineService>();
    services.AddScoped<ITimeZoneService, TimeZoneService>();
    services.AddScoped<IVnPayIntegrationService, VnPayIntegrationService>();
    services.AddScoped<IErrorFactory, ErrorFactory>();
    services.AddScoped<IReportService, ReportService>();
    services.AddScoped<IEventService, EventService>();
    services.AddScoped<IStoreService, StoreService>(); // Add Store Service

    // Add HttpClient for GHNService
    services.AddHttpClient<IGHNService, GHNService>();

    // Add HttpClient for GeminiService
    services.AddScoped<IChatService, ChatService>();

    #region Add Strategies

    services.AddSingleton<IBlogFilterStrategy, ContentFilterStrategy>();
    services.AddSingleton<IBlogFilterStrategy, TitleFilterStrategy>();
    services.AddSingleton<IBlogFilterStrategy, StaffUsernameFilterStrategy>();
    services.AddSingleton<IBlogFilterStrategy, SortOrderFilterStrategy>();

    services.AddSingleton<IReportGenerateStrategy, PdfReportGenerateStrategy>();
    services.AddSingleton<IReportGenerateStrategy, WordReportGenerateStrategy>();
    services.AddSingleton<IReportTypeStrategy, RevenueReportStrategy>();
    services.AddSingleton<IReportTypeStrategy, ProductPerformanceReportStrategy>();

    services.AddSingleton<IInvoiceGenerateStrategy, WalkInInvoiceStrategy>();
    services.AddSingleton<IInvoiceGenerateStrategy, OnlineInvoiceStrategy>();

    services.AddSingleton<ICosmeticFilterStrategy, NameFilterStrategy>();
    services.AddSingleton<ICosmeticFilterStrategy, BrandFilterStrategy>();
    services.AddSingleton<ICosmeticFilterStrategy, SkinTypeFilterStrategy>();
    services.AddSingleton<ICosmeticFilterStrategy, CosmeticTypeFilterStrategy>();
    services.AddSingleton<ICosmeticFilterStrategy, GenderFilterStrategy>();
    services.AddScoped<ICosmeticFilterStrategy, PriceFilterStrategy>();

    services.AddSingleton<IInvoiceGenerateStrategy, WalkInInvoiceStrategy>();
    services.AddSingleton<IInvoiceGenerateStrategy, OnlineInvoiceStrategy>();

    #endregion

    #region Add Factories 
    services.AddScoped<IFilePathFactory, FilePathFactory>();
    #endregion

    services.AddScoped<IClaimsService, ClaimsService>();
    return services;
  }
}