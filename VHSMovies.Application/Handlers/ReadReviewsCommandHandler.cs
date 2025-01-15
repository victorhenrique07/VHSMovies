using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Commands;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Application.Handlers
{
    public class ReadReviewsCommandHandler : IRequestHandler<ReadReviewsCommand, Unit>
    {
        private readonly IReviewRepository reviewRepository;
        private readonly ILogger<ReadReviewsCommandHandler> _logger;

        public ReadReviewsCommandHandler(ILogger<ReadReviewsCommandHandler> _logger, IReviewRepository reviewRepository)
        {
            this._logger = _logger;
            this.reviewRepository = reviewRepository;
        }

        public async Task<Unit> Handle(ReadReviewsCommand command, CancellationToken cancellationToken)
        {
            IEnumerable<Review> reviews = await reviewRepository.GetAll();

            List<Review> reviewsToUpdate = new List<Review>();

            foreach (var rows in command.ReviewsRows)
            {
                int titleId = 0;
                decimal rating = 0;

                foreach (var row in rows)
                {
                    if (row.Key.ToLower() == "filmid")
                        titleId = !string.IsNullOrEmpty(row.Value) ? Convert.ToInt32(row.Value) : 0;
                    if (row.Key.ToLower() == "vote_average")
                        rating = !string.IsNullOrEmpty(row.Value) ? Convert.ToDecimal(row.Value) : 0m;
                }

                Review review = reviews.Where(r => r.TitleId == titleId).FirstOrDefault();

                if (review != null)
                {
                    review.Rating = rating;

                    reviewsToUpdate.Add(review);
                }
            }

            await reviewRepository.UpdateReviews(reviewsToUpdate);

            return Unit.Value;
        }
    }
}
