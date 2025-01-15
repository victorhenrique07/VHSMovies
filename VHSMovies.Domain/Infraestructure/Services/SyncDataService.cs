using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium.BiDi.Modules.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Domain.Infraestructure.Services
{
    public class SyncDataService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SyncDataService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task UpdateTitlesAsync(string reviewerName)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var reviewRepository = scope.ServiceProvider.GetRequiredService<IReviewRepository>();

                    List<Review> reviews = await reviewRepository.GetByReviewerName(reviewerName);

                    List<Review> reviewsToUpdate = new List<Review>();

                    foreach (Review review in reviews)
                    {
                        Review updatedReview = ReadTitleReview(review.TitleExternalId, reviewerName);

                        if (review.Rating == updatedReview.Rating)
                            continue;

                        review.Rating = updatedReview.Rating;

                        reviewsToUpdate.Add(review);
                    }

                    if (reviewsToUpdate.Any())
                    {
                        await reviewRepository.UpdateReviews(reviewsToUpdate);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao atualizar os títulos: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task UpdateTitlesGenres(string reviewerName)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var reviewRepository = scope.ServiceProvider.GetRequiredService<IReviewRepository>();
                    var titleRepository = scope.ServiceProvider.GetRequiredService<ITitleRepository<Title>>();

                    IEnumerable<Review> reviews = await reviewRepository.GetByReviewerName(reviewerName);

                    List<Title> titlesGenresToUpdate = new List<Title>();

                    foreach (Review review in reviews)
                    {
                        List<TitleGenre> genres = ReadTitleGenres(review.TitleExternalId, review.Reviewer, review.TitleId);

                        review.Title.Genres = genres;

                        titlesGenresToUpdate.Add(review.Title);
                    }

                    await titleRepository.UpdateAsync(titlesGenresToUpdate);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao atualizar os títulos: {ex.Message}");
                    throw;
                }
            }
        }

        private Review ReadTitleReview(string externalId, string sourceName)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dataReaders = scope.ServiceProvider.GetRequiredService<IEnumerable<IDataReader>>();

                IDataReader dataReader = dataReaders.Single(r => r.GetSourceName() == sourceName);

                string url = dataReader.GetFullUrl(externalId);

                Review review = dataReader.ReadReview(url);

                Console.WriteLine($"Titulo sendo lido: {review.Title.Name} - {externalId}");

                return review;
            }
        }

        private List<TitleGenre> ReadTitleGenres(string externalId, string sourceName, int titleId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dataReaders = scope.ServiceProvider.GetRequiredService<IEnumerable<IDataReader>>();

                IDataReader dataReader = dataReaders.Single(r => r.GetSourceName() == sourceName.ToLower());

                string url = dataReader.GetFullUrl(externalId);

                List<TitleGenre> genres = dataReader.ReadGenres(url, titleId);

                return genres;
            }
        }
    }
}
