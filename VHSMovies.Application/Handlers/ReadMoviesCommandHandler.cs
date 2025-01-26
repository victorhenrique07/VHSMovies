using MediatR;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium.DevTools.V129.Audits;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Commands;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Application.Handlers
{
    public class ReadMoviesCommandHandler : IRequestHandler<ReadMoviesCommand, Unit>
    {
        private readonly ITitleRepository<Title> titleRepository;
        private readonly IGenreRepository genreRepository;
        private readonly ILogger<ReadMoviesCommandHandler> _logger;



        public ReadMoviesCommandHandler(ITitleRepository<Title> titleRepository,
            IGenreRepository genreRepository,
            ILogger<ReadMoviesCommandHandler> _logger)
        {
            this.titleRepository = titleRepository;
            this.genreRepository = genreRepository;
            this._logger = _logger;
        }

        public async Task<Unit> Handle(ReadMoviesCommand command, CancellationToken cancellationToken)
        {
            List<Title> titles = new List<Title>();

            List<TitleImages> images = new List<TitleImages>();

            var teste = await titleRepository.GetAllByReviewerName("imdb");

            IEnumerable<Genre> genres = await genreRepository.GetAll();

            var existingIds = new HashSet<int>((await titleRepository.GetAll()).Select(x => x.Id));

            List<string> validHeaders = new List<string>()
            {
                "id",
                "title",
                "runtime",
                "backdrop_path",
                "tconst",
                "overview",
                "poster_path",
                "genres",
                "averageRating",
                "numVotes"
            };

            foreach (var rows in command.TitlesRows)
            {
                int id = 0;
                string name = "";
                string description = "";
                string IMDbId = "";
                int runTime = 0;
                string poster_path = "";
                string backdrop_path = "";
                decimal ratingAverage = 0.0m;
                int ratingsCount = 0;
                List<string> titleGenres = new List<string>();

                bool matchKeys = validHeaders.All(header =>
                    rows.Any(r => r.Key.Trim().ToLower() == header.Trim().ToLower())
                );

                if (!matchKeys)
                    throw new KeyNotFoundException("Cabeçalhos não correspondentes.");

                Dictionary<string, Action<string>> headerToPropertyMap = new Dictionary<string, Action<string>>
                {
                    { "id", value => id = ParseInt(value) },
                    { "title", value => name = GetValueOrDefault(value) },
                    { "runtime", value => runTime = ParseInt(value) },
                    { "backdrop_path", value => backdrop_path = GetValueOrDefault(value) },
                    { "tconst", value => IMDbId = GetValueOrDefault(value) },
                    { "overview", value => description = GetValueOrDefault(value) },
                    { "poster_path", value => poster_path = GetValueOrDefault(value) },
                    { "genres", value => titleGenres = !string.IsNullOrEmpty(value) ? value.Split(",").Select(g => g.Trim()).ToList() : new List<string>() },
                    { "averagerating", value => ratingAverage = ParseDecimal(value) },
                    { "numvotes", value => ratingsCount = ParseInt(value) },
                };

                foreach (var row in rows)
                {
                    var key = row.Key.Trim().ToLower();
                    if (headerToPropertyMap.ContainsKey(key))
                    {
                        headerToPropertyMap[key](row.Value);
                    }
                }

                _logger.LogInformation($"Processando filme: {id} - {name}");

                bool titleExists = existingIds.Contains(id);

                if (titleExists)
                {
                    _logger.LogInformation($"Filme {id} - {name} já existe.");

                    continue;
                }

                List<TitleGenre> titlesGenres = new List<TitleGenre>();

                foreach (string genre in titleGenres)
                {
                    Genre genreExists = genres.FirstOrDefault(g => string.Equals(g.Name.Trim(), genre, StringComparison.OrdinalIgnoreCase));

                    if (genreExists == null)
                        continue;

                    TitleGenre titleGenre = new TitleGenre()
                    {
                        Genre = genreExists,
                        GenreId = genreExists.Id
                    };

                    titlesGenres.Add(titleGenre);
                }

                Review review = new Review("IMDb", ratingAverage, ratingsCount)
                {
                    TitleExternalId = IMDbId,
                    TitleId = id
                };

                Movie movie = new Movie(name, description, backdrop_path, poster_path, new List<Review>() { review }, runTime)
                {
                    Id = id,
                    Genres = titlesGenres
                };

                titles.Add(movie);
            }

            titles = titles.DistinctBy(t => t.Id).ToList();

            await titleRepository.RegisterListAsync(titles);

            return Unit.Value;
        }

        private static string GetValueOrDefault(string value, string defaultValue = "")
        {
            return !string.IsNullOrEmpty(value) ? value : defaultValue;
        }
        private static int ParseInt(string value)
        {
            return int.TryParse(value, out var result) ? result : 0;
        }
        private static decimal ParseDecimal(string value)
        {
            return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : 0.0m;
        }
    }
}
