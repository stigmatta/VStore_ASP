

using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.Extensions.Logging;

namespace Business_Logic.Services
{
    public class ReviewService
    {
        private readonly IUnitOfWork Database;
        private readonly ILogger<ReviewService> _logger;
        public ReviewService(IUnitOfWork unitOfWork, ILogger<ReviewService> logger)
        {
            Database = unitOfWork;
            _logger = logger;
        }
        public async Task AddReview(Review review)
        {
            await Database.ReviewRepository.Add(review);
            await Database.Save();
        }
        public async Task<IEnumerable<Review>> GetAll(Guid gameId)
        {
            var reviews = await Database.ReviewRepository.GetAll(gameId);
            return reviews;
        }

        public int GetRatingInPercent(IEnumerable<Review> reviews)
        {
            if (reviews == null || !reviews.Any())
            {
                return 0;
            }

            int positiveCount = reviews.Count(r => r.IsLiked);
            _logger.LogWarning($"Pos: {positiveCount}");
            int negativeCount = reviews.Count(r => !r.IsLiked);
            _logger.LogWarning($"Neg: {negativeCount}");

            int totalCount = reviews.Count();

            int ratingInPercent = (int)(positiveCount / (double)totalCount * 100);

            return ratingInPercent;
        }

    }
}
