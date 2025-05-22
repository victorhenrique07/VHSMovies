using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using OpenQA.Selenium.DevTools.V129.Audits;

using VHSMovies.Application.Commands;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Mediator;
using VHSMovies.Mediator.Implementation;
using VHSMovies.Mediator.Interfaces;

namespace VHSMovies.Application.Handlers
{
    public class ReadMoviesCommandHandler : IRequestHandler<ReadMoviesCommand, Unit>
    {
        private readonly ITitleRepository titleRepository;
        private readonly IGenreRepository genreRepository;
        private readonly ILogger<ReadMoviesCommandHandler> _logger;



        public ReadMoviesCommandHandler(ITitleRepository titleRepository,
            IGenreRepository genreRepository,
            ILogger<ReadMoviesCommandHandler> _logger)
        {
            this.titleRepository = titleRepository;
            this.genreRepository = genreRepository;
            this._logger = _logger;
        }

        public async Task<Unit> Handle(ReadMoviesCommand command, CancellationToken cancellationToken)
        {
            var titles = new List<Title>();
            var genres = await genreRepository.GetAll();

            var genreLookup = genres.ToDictionary(
                g => g.Name.Trim().ToLower(), g => g, StringComparer.OrdinalIgnoreCase
            );

            var existingIds = new HashSet<string>(
                titleRepository.Query().Select(x => x.IMDB_Id),
                StringComparer.OrdinalIgnoreCase
            );

            var validHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "tconst", "titleType", "primaryTitle", "startYear", "runtimeMinutes", "genres",
            };

            var processedIds = new HashSet<string>();

            foreach (var rows in command.TitlesRows)
            {
                var values = rows.ToDictionary(
                    kv => kv.Key.Trim().ToLower(),
                    kv => kv.Value?.Trim() ?? string.Empty
                );

                if (!validHeaders.IsSubsetOf(values.Keys))
                    throw new KeyNotFoundException("Headers do not match.");

                string tconst = values["tconst"];
                if (!processedIds.Add(tconst) || existingIds.Contains(tconst))
                {
                    _logger.LogInformation($"Title {tconst} already exists.");
                    continue;
                }

                string primaryTitle = values["primarytitle"];
                TitleType titleType = GetTitleType(values["titletype"]);
                int startYear = ParseInt(values["startyear"]);
                int runtimeMinutes = ParseInt(values["runtimeminutes"]);

                var titleGenres = values["genres"]
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Where(g => genreLookup.ContainsKey(g.ToLower()))
                    .Select(g => new TitleGenre
                    {
                        Genre = genreLookup[g.ToLower()]
                    })
                    .ToList();

                var title = new Title(primaryTitle, titleType, startYear, tconst)
                {
                    Runtime = runtimeMinutes,
                    Genres = titleGenres
                };

                titles.Add(title);
                _logger.LogInformation($"Processing title: {tconst} - {primaryTitle}");
            }

            await titleRepository.RegisterListAsync(titles);
            await titleRepository.SaveChangesAsync();

            return Unit.Value;
        }


        private static TitleType GetTitleType(string titleType)
        {
            return titleType.ToLower() switch
            {
                "movie" => TitleType.Movie,
                "tvseries" => TitleType.TvSeries,
                "tvmovie" => TitleType.Movie,
                "tvminiseries" => TitleType.TvSeries
            };
        }
        private static string GetValueOrDefault(string? value) =>
            string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();

        private static int ParseInt(string value) =>
            int.TryParse(value, out var result) ? result : 0;
    }
}
