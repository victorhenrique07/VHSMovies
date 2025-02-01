using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Commands;
using VHSMovies.Application.Factories;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Application.Handlers
{
    public class GetMostRelevantTitlesQueryHandler : IRequestHandler<GetMostRelevantTitlesQuery, IReadOnlyCollection<TitleResponse>>
    {
        private readonly IRecomendedTitlesRepository recommendedTitlesRepository;

        public GetMostRelevantTitlesQueryHandler(IRecomendedTitlesRepository recommendedTitlesRepository)
        {
            this.recommendedTitlesRepository = recommendedTitlesRepository;
        }
        public async Task<IReadOnlyCollection<TitleResponse>> Handle(GetMostRelevantTitlesQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<RecommendedTitle> titles = await recommendedTitlesRepository.GetAllRecommendedTitles();

            if (query.GenreName != null)
            {
                return titles
                    .Where(t => t.Genres.ToLower().Contains(query.GenreName))
                    .OrderByDescending(t => t.Relevance)
                    .Take(10)
                    .Select(t =>
                        new TitleResponse(t.Id, t.Name, t.Description, t.AverageRating, t.TotalReviews)
                        {
                            PrincipalImageUrl = t.PrincipalImageUrl,
                            PosterImageUrl = t.PosterImageUrl,
                            Genres = t.Genres
                                .Split(new[] { ", " }, StringSplitOptions.None)
                                .ToList()
                                .Select(gt => new GenreResponse(gt)).ToList()
                        })
                    .ToList();
            }

            TitleResponseFactory titleResponseFactory = new TitleResponseFactory();

            return titles
                    .OrderByDescending(t => t.Relevance)
                    .Take(10)
                    .Select(t => titleResponseFactory.CreateTitleResponseByRecommendedTitle(t))
                    .ToList();
        }
    }
}
