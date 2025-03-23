using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VHSMovies.Application.Commands;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Application.Handlers
{
    public class ReadTitlesGenresCommandHandler : IRequestHandler<ReadTitlesGenresCommand, Unit>
    {
        private readonly ITitleGenreRepository titleGenreRepository;
        private readonly ITitleRepository<Title> titleRepository;
        private readonly ILogger<ReadTitlesGenresCommandHandler> _logger;

        public ReadTitlesGenresCommandHandler(ILogger<ReadTitlesGenresCommandHandler> _logger, ITitleGenreRepository titleGenreRepository, ITitleRepository<Title> titleRepository)
        {
            this._logger = _logger;
            this.titleGenreRepository = titleGenreRepository;
            this.titleRepository = titleRepository;
        }

        public async Task<Unit> Handle(ReadTitlesGenresCommand command, CancellationToken cancellationToken)
        {
            List<TitleGenre> titleGenres = new List<TitleGenre>();

            var existingTitles = await titleRepository.GetAll();

            List<string> validHeaders = new List<string>()
            {
                "filmid",
                "genreid"
            };

            foreach (var rows in command.GenresRows)
            {
                int titleId = 0;
                int genreId = 0;

                bool matchKeys = validHeaders.All(header => rows.Any(r => r.Key.ToLower() == header));

                if (!matchKeys)
                    throw new KeyNotFoundException("Cabeçalhos não correspondentes.");

                foreach (var row in rows)
                {
                    if (row.Key.ToLower() == "filmid")
                        titleId = !string.IsNullOrEmpty(row.Value) ? Convert.ToInt32(row.Value) : 0;
                    if (row.Key.ToLower() == "genreid")
                        genreId = !string.IsNullOrEmpty(row.Value) ? Convert.ToInt32(row.Value) : 0;
                }

                TitleGenre titleGenre = new TitleGenre()
                {
                    TitleId = titleId,
                    GenreId = genreId,
                };

                if (titleGenres.Any(r => r.TitleId == titleId && r.GenreId == genreId))
                    continue;

                if (!existingTitles.Any(t => t.Id == titleId))
                    continue;

                titleGenres.Add(titleGenre);
            }

            await titleGenreRepository.RegisterGenresList(titleGenres);

            return Unit.Value;
        }
    }
}
