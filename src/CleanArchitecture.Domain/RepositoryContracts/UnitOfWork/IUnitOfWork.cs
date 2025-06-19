using Microsoft.EntityFrameworkCore.Storage;

namespace CleanArchitecture.Domain.RepositoryContracts.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
  #region Repositories
  IBatchRepository Batches { get; }
  IBlogRepository Blogs { get; } 
  IBlogTagRepository BlogTags { get; }
  IBrandRepository Brands { get; }
  ICartItemRepository CartItems { get; }
  ICartRepository Carts { get; }
  ICategoryRepository Categories { get; }
  ICompanyInformationRepository CompanyInformation { get; }
  ICosmeticImageRepository CosmeticImages { get; }
  ICosmeticRepository Cosmetics { get; }
  ICosmeticPriceRepository CosmeticPrices { get; }
  ICosmeticSubCategoryRepository CosmeticSubCategories { get; }
  ICosmeticTypeRepository CosmeticTypes { get; }
  ICouponRepository Coupons { get; }
  IEventRepository Events { get; }
  IFAQRepository FAQs { get; }
  IFeedbackRepository Feedbacks { get; }
  IOrderItemRepository OrderItems { get; }
  IOrderRepository Orders { get; }
  IPaymentRepository Payments { get; }
  IPolicyRepository Policies { get; } 
  IQuestionOptionRepository QuestionOptions { get; }
  IQuestionRepository Questions { get; }
  IQuestionTypeRepository QuestionTypes { get; }
  IQuizRepository Quizs { get; }
  IQuizResultRepository QuizResults { get; }
  IQuizAnswerRepository QuizAnswers { get; }
  IRefundRepository Refunds { get; }
  IRefundItemRepository RefundItems { get; }
  IRoutineRepository Routines { get; }
  IRoutineStepRepository RoutineSteps { get; } 
  ISkinTypeRepository SkinTypes { get; }
  IStoreRepository Stores { get; } // Add Store Repository
  ISubCategoryRepository SubCategories { get; }
  ITagRepository Tags { get; }
  ITestimonialRepository Testimonials { get; }
  IUserRepository Users { get; }
  IUserCouponRepository UserCoupons { get; }
  IPlayLogRepository PlayLogs { get; }
  #endregion
  Task<IDbContextTransaction> BeginTransactionAsync();
  Task RollBackAsync();
  Task<bool> CompleteAsync();
}
