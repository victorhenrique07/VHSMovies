using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
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
        private readonly IGenreRepository genreRepository;
        private readonly IRecommendedTitlesRepository recommendedTitlesRepository;

        public GetMostRelevantTitlesQueryHandler(IRecommendedTitlesRepository recommendedTitlesRepository, IGenreRepository genreRepository)
        {
            this.recommendedTitlesRepository = recommendedTitlesRepository;
            this.genreRepository = genreRepository;
        }
        public async Task<IReadOnlyCollection<TitleResponse>> Handle(GetMostRelevantTitlesQuery query, CancellationToken cancellationToken)
        {
            TitleResponseFactory titleResponseFactory = new TitleResponseFactory();

            IQueryable<RecommendedTitle> titles = recommendedTitlesRepository.Query();

            IReadOnlyCollection<TitleResponse> response = new List<TitleResponse>();

            IReadOnlyCollection<Genre> allGenres = await genreRepository.GetAll();

            if (query.TitlesToExclude != null)
            {
                titles = titles.AsEnumerable().ExceptBy(query.TitlesToExclude, t => t.Id).AsQueryable();
            }

            if (query.GenresId != null && query.GenresId.Any())
            {
                var genresToQuery = allGenres
                    .Where(g => query.GenresId.Contains(g.Id))
                    .Select(g => g.Name);

                titles = titles.Where(t => genresToQuery.Any(genre => t.Genres.Contains(genre)));
            }

            IReadOnlyCollection<RecommendedTitle> data = titles
                .AsEnumerable()
                .OrderByDescending(t => t.Relevance)
                .Take(query.TitlesAmount)
                .ToList();

            response = data
                .Select(t => titleResponseFactory.CreateTitleResponseByRecommendedTitle(t, allGenres)).ToList();

            return response;
        }
    }
}
