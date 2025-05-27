using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using VHSMovies.Application.Commands;
using VHSMovies.Application.Factories;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Infrastructure.Services;
using VHSMovies.Mediator;
using VHSMovies.Mediator.Interfaces;

namespace VHSMovies.Application.Handlers
{
    public class GetTitlesBySearchQueryHandler : IRequestHandler<GetTitlesBySearchQuery, IReadOnlyCollection<TitleResponse>>
    {
        private readonly IRecommendedTitlesRepository titleRepository;
        private readonly ITMDbService tmdbService;

        public GetTitlesBySearchQueryHandler(IRecommendedTitlesRepository titleRepository, ITMDbService tmdbService)
        {
            this.titleRepository = titleRepository;
            this.tmdbService = tmdbService;
        }

        public async Task<IReadOnlyCollection<TitleResponse>> Handle(GetTitlesBySearchQuery query, CancellationToken cancellationToken)
        {
            TitleResponseFactory titleResponseFactory = new TitleResponseFactory(tmdbService);

            IQueryable<RecommendedTitle> titles = titleRepository.Query()
                .Where(p => EF.Functions.Like(p.Name.ToLower(), $"%{query.SearchQuery.ToLower()}%"))
                .Take(5);

            var results = titles.ToList();

            List<TitleResponse> response = results.Select(t => titleResponseFactory.CreateTitleResponseByRecommendedTitle(t)).ToList();

            return response;
        }
    }
}
