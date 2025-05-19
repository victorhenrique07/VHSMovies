using VHSMovies.Mediator;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Commands;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Mediator.Interfaces;
using VHSMovies.Mediator.Implementation;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace VHSMovies.Application.Handlers
{
    public class ReadReviewsCommandHandler : IRequestHandler<ReadReviewsCommand, Unit>
    {
        private readonly IReviewRepository reviewRepository;
        private readonly ITitleRepository titleRepository;
        private readonly ILogger<ReadReviewsCommandHandler> _logger;

        public ReadReviewsCommandHandler(ILogger<ReadReviewsCommandHandler> _logger, IReviewRepository reviewRepository, ITitleRepository titleRepository)
        {
            this._logger = _logger;
            this.reviewRepository = reviewRepository;
            this.titleRepository = titleRepository;
        }

        public async Task<Unit> Handle(ReadReviewsCommand command, CancellationToken cancellationToken)
        {
            var reviews = new List<Review>();

            var validHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "tconst", "averageRating", "numVotes"
            };

            var tconsts = command.ReviewsRows
                .SelectMany(r => r)
                .Where(p => p.Key.Equals("tconst", StringComparison.OrdinalIgnoreCase))
                .Select(p => p.Value?.Trim())
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .ToHashSet();

            var titleList = await titleRepository.Query()
                .Where(t => tconsts.Contains(t.IMDB_Id))
                .ToListAsync(cancellationToken);

            var titleMap = titleList.ToDictionary(t => t.IMDB_Id);

            foreach (var rowSet in command.ReviewsRows)
            {
                var normalizedKeys = rowSet.Select(r => r.Key.Trim().ToLower()).ToHashSet();

                if (!validHeaders.IsSubsetOf(normalizedKeys))
                    throw new KeyNotFoundException("Headers do not match.");

                var values = rowSet.ToDictionary(
                    kv => kv.Key.Trim().ToLower(),
                    kv => kv.Value?.Trim() ?? string.Empty
                );

                string tconst = GetValueOrDefault(values["tconst"]);
                decimal rating = ParseDecimal(values.GetValueOrDefault("averagerating"));
                int numVotes = ParseInt(values.GetValueOrDefault("numvotes"));

                if (!titleMap.TryGetValue(tconst, out var title))
                {
                    _logger.LogWarning($"Title with tconst {tconst} not found.");
                    continue;
                }

                _logger.LogInformation($"Processing review to: {tconst} - {title.Name} - {rating}");

                Review review = new Review(command.Source, rating, numVotes)
                {
                    TitleExternalId = tconst
                };

                reviews.Add(review);

                title.Ratings.Add(review);
                title.SetRelevance();
            }

            await reviewRepository.AddReviews(reviews);
            await reviewRepository.SaveChangesAsync();
            await titleRepository.SaveChangesAsync();

            return Unit.Value;
        }

        private static string GetValueOrDefault(string? value) =>
            string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();

        private static decimal ParseDecimal(string value) =>
            decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : 0.0m;

        private static int ParseInt(string value) =>
            int.TryParse(value, out var result) ? result : 0;
    }
}
