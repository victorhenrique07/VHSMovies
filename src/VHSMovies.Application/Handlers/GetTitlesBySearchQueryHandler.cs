using MediatR;
using Microsoft.EntityFrameworkCore;
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
    class GetTitlesBySearchQueryHandler : IRequestHandler<GetTitlesBySearchQuery, IReadOnlyCollection<TitleResponse>>
    {
        private readonly IRecomendedTitlesRepository titleRepository;

        public GetTitlesBySearchQueryHandler(IRecomendedTitlesRepository titleRepository)
        {
            this.titleRepository = titleRepository;
        }

        public async Task<IReadOnlyCollection<TitleResponse>> Handle(GetTitlesBySearchQuery query, CancellationToken cancellationToken)
        {
            TitleResponseFactory titleResponseFactory = new TitleResponseFactory();

            var results = await titleRepository.Query()
                .Where(p => EF.Functions.Like(p.Name.ToLower(), $"%{query.SearchQuery.ToLower()}%"))
                .Take(5)
                .ToListAsync();

            List<TitleResponse> response = results.Select(t => titleResponseFactory.CreateTitleResponseByRecommendedTitle(t)).ToList();

            return response;
        }
    }
}
