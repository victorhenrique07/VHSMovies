using OpenQA.Selenium;
using VHSMovies.Application.Commands;
using VHSMovies.Application.Factories;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Mediator.Interfaces;

namespace VHSMovies.Application.Handlers
{
    public class GetTitleByIdQueryHandler : IRequestHandler<GetTitleByIdQuery, TitleResponse>
    {
        private readonly IRecommendedTitlesRepository recommendedTitlesRepository;
        private readonly IGenreRepository genreRepository;

        public GetTitleByIdQueryHandler(IRecommendedTitlesRepository recommendedTitlesRepository, IGenreRepository genreRepository)
        {
            this.recommendedTitlesRepository = recommendedTitlesRepository;
            this.genreRepository = genreRepository;
        }
        public async Task<TitleResponse?> Handle(GetTitleByIdQuery query, CancellationToken cancellationToken)
        {
            RecommendedTitle title = await recommendedTitlesRepository.GetById(query.Id);

            if (title == null)
                return null;

            IReadOnlyCollection<Genre> allGenres = await genreRepository.GetAll();

            TitleResponseFactory titleResponseFactory = new TitleResponseFactory();

            TitleResponse response = titleResponseFactory.CreateTitleResponseByRecommendedTitle(title, allGenres);

            return response;
        }
    }
}
