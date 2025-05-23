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
        private readonly ITitleRepository titleRepository;

        public GetTitleByIdQueryHandler(ITitleRepository titleRepository)
        {
            this.titleRepository = titleRepository;
        }

        public async Task<TitleResponse?> Handle(GetTitleByIdQuery query, CancellationToken cancellationToken)
        {
            Title title = await titleRepository.GetByIdAsync(query.Id);

            if (title == null)
                return null;

            TitleResponseFactory titleResponseFactory = new TitleResponseFactory();

            TitleResponse response = titleResponseFactory.CreateTitleResponseByTitle(title);

            return response;
        }
    }
}
