// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Infrastructure/Repositories/UnitOfWork/UnitOfWork.cs
using CleanArchitecture.Application.ServiceContracts;
using CleanArchitecture.Domain.RepositoryContracts;
using CleanArchitecture.Domain.RepositoryContracts.UnitOfWork;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Repositories.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
  private readonly ApplicationDbContext _context;
  private IDbContextTransaction _transaction;
  private bool _commited;
  private readonly ILogger<UnitOfWork> _logger;
  private readonly ITimeZoneService _timeZoneService;

  #region Repositories

  public IBatchRepository Batches => new BatchRepository(_context);
  public IBlogRepository Blogs => new BlogRepository(_context);
  public IBlogTagRepository BlogTags => new BlogTagRepository(_context);
  public IBrandRepository Brands => new BrandRepository(_context);
  public ICartItemRepository CartItems => new CartItemRepository(_context);
  public ICartRepository Carts => new CartRepository(_context);
  public ICategoryRepository Categories => new CategoryRepository(_context);
  public ICompanyInformationRepository CompanyInformation => new CompanyInformationRepository(_context);
  public ICosmeticImageRepository CosmeticImages => new CosmeticImageRepository(_context);
  public ICosmeticRepository Cosmetics => new CosmeticRepository(_context);
  public ICosmeticPriceRepository CosmeticPrices => new CosmeticPriceRepository(_context);
  public ICosmeticSubCategoryRepository CosmeticSubCategories => new CosmeticSubCategoryRepository(_context);
  public ICosmeticTypeRepository CosmeticTypes => new CosmeticTypeRepository(_context);
  public ICouponRepository Coupons => new CouponRepository(_context);
  public IEventRepository Events => new EventRepository(_context);
  public IFAQRepository FAQs => new FAQRepository(_context);
  public IFeedbackRepository Feedbacks => new FeedbackRepository(_context);
  public IOrderItemRepository OrderItems => new OrderItemRepository(_context);
  public IOrderRepository Orders => new OrderRepository(_context);
  public IPaymentRepository Payments => new PaymentRepository(_context);
  public IPolicyRepository Policies => new PolicyRepository(_context);
  public IQuestionOptionRepository QuestionOptions => new QuestionOptionRepository(_context);
  public IQuestionRepository Questions => new QuestionRepository(_context);
  public IQuestionTypeRepository QuestionTypes => new QuestionTypeRepository(_context);
  public IQuizRepository Quizs => new QuizRepository(_context);
  public IQuizResultRepository QuizResults => new QuizResultRepository(_context);
  public IQuizAnswerRepository QuizAnswers => new QuizAnswerRepository(_context);
  public IRefundRepository Refunds => new RefundRepository(_context);
  public IRefundItemRepository RefundItems => new RefundItemRepository(_context);
  public IRoutineRepository Routines => new RoutineRepository(_context);
  public IRoutineStepRepository RoutineSteps => new RoutineStepRepository(_context);
  public ISkinTypeRepository SkinTypes => new SkinTypeRepository(_context);
  public IStoreRepository Stores => new StoreRepository(_context); // Add Store Repository Implementation
  public ISubCategoryRepository SubCategories => new SubCategoryRepository(_context);
  public ITagRepository Tags => new TagRepository(_context);
  public ITestimonialRepository Testimonials => new TestimonialRepository(_context);
  public IUserRepository Users => new UserRepository(_context);
  public IUserCouponRepository UserCoupons => new UserCouponRepository(_context);
  public IPlayLogRepository PlayLogs => new PlayLogRepository(_context);

  #endregion

  public UnitOfWork(ApplicationDbContext context, ILogger<UnitOfWork> logger, ITimeZoneService timeZoneService)
  {
    _context = context;
    _logger = logger;
    _timeZoneService = timeZoneService;
    _transaction = _context.Database.BeginTransaction();
  }

  public async Task RollBackAsync()
  {
    await _transaction.RollbackAsync();
  }

  public async Task<bool> CompleteAsync()
  {
    try
    {
      await _context.SaveChangesAsync();
      await _transaction.CommitAsync();
      return true;
    }
    catch (Exception ex)
    {
      // FIX: Ensure the rollback operation is awaited to prevent potential race conditions.
      await _transaction.RollbackAsync();
      _logger.LogError($"Database saved failed at {_timeZoneService.ConvertToLocalTime(DateTime.UtcNow)}\n" +
                       $"with error: {ex.Message}");
      return false;
    }
  }
  public async Task<IDbContextTransaction> BeginTransactionAsync()
  {
    return await _context.Database.BeginTransactionAsync();
  }
  public void Dispose()
  {
    _transaction?.Dispose();
    _transaction = null;
  }
}