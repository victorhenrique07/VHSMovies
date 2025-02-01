using MediatR;
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
    public class GetTitleByIdQueryHandler : IRequestHandler<GetTitleByIdQuery, TitleResponse>
    {
        private readonly IRecomendedTitlesRepository recommendedTitlesRepository;

        public GetTitleByIdQueryHandler(IRecomendedTitlesRepository recommendedTitlesRepository)
        {
            this.recommendedTitlesRepository = recommendedTitlesRepository;
        }
        public async Task<TitleResponse> Handle(GetTitleByIdQuery query, CancellationToken cancellationToken)
        {
            RecommendedTitle title = await recommendedTitlesRepository.GetById(query.Id);

            title.AverageRating = title.AverageRating / 2;

            TitleResponseFactory titleResponseFactory = new TitleResponseFactory();

            return titleResponseFactory.CreateTitleResponseByRecommendedTitle(title);
        }
    }
}
