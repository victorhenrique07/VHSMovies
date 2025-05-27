using OpenQA.Selenium;

using VHSMovies.Application.Commands;
using VHSMovies.Application.Factories;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Infrastructure.Services;
using VHSMovies.Mediator.Interfaces;

namespace VHSMovies.Application.Handlers
{
    public class GetTitleByIdQueryHandler : IRequestHandler<GetTitleByIdQuery, TitleResponse>
    {
        private readonly ITitleRepository titleRepository;
        private readonly ITMDbService tmdbService;

        public GetTitleByIdQueryHandler(ITitleRepository titleRepository, ITMDbService tmdbService)
        {
            this.titleRepository = titleRepository;
            this.tmdbService = tmdbService;
        }

        public async Task<TitleResponse?> Handle(GetTitleByIdQuery query, CancellationToken cancellationToken)
        {
            Title title = await titleRepository.GetByIdAsync(query.Id);

            if (title == null)
                return null;

            TitleResponseFactory titleResponseFactory = new TitleResponseFactory(tmdbService);

            TitleResponse response = titleResponseFactory.CreateTitleResponseByTitle(title);

            return response;
        }
    }
}
