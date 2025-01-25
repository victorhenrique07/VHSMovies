using MediatR;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium.DevTools.V129.Audits;
using System;
using System.Collections.Generic;
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
        private readonly ILogger<ReadMoviesCommandHandler> _logger;

        public ReadMoviesCommandHandler(ITitleRepository<Title> titleRepository,
            ILogger<ReadMoviesCommandHandler> _logger)
        {
            this.titleRepository = titleRepository;
            this._logger = _logger;
        }

        public async Task<Unit> Handle(ReadMoviesCommand command, CancellationToken cancellationToken)
        {
            List<Title> titles = new List<Title>();

            List<TitleImages> images = new List<TitleImages>();

            var teste = await titleRepository.GetAllByReviewerName("imdb");

            var existingIds = new HashSet<int>((await titleRepository.GetAll()).Select(x => x.Id));

            List<string> validHeaders = new List<string>()
            {
                "filmid",
                "title",
                "backdrop_path",
                "overview",
                "poster_path",
                "imdb_id",
                "runtime",
                "vote_average",
                "vote_count"
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

                bool matchKeys = validHeaders.All(header => rows.Any(r => r.Key.ToLower() == header));

                if (!matchKeys)
                    throw new KeyNotFoundException("Cabeçalhos não correspondentes.");

                foreach (var row in rows)
                {
                    if (row.Key.ToLower() == "filmid")
                        id = !string.IsNullOrEmpty(row.Value) ? Convert.ToInt32(row.Value) : 0;
                    if (row.Key.ToLower() == "title")
                        name = !string.IsNullOrEmpty(row.Value) ? row.Value : "";
                    if (row.Key.ToLower() == "imdb_id")
                        IMDbId = !string.IsNullOrEmpty(row.Value) ? row.Value : "";
                    if (row.Key.ToLower() == "backdrop_path")
                        backdrop_path = !string.IsNullOrEmpty(row.Value) ? row.Value : "";
                    if (row.Key.ToLower() == "overview")
                        description = !string.IsNullOrEmpty(row.Value) ? row.Value : "";
                    if (row.Key.ToLower() == "poster_path")
                        poster_path = !string.IsNullOrEmpty(row.Value) ? row.Value : "";
                    if (row.Key.ToLower() == "vote_average")
                        ratingAverage = !string.IsNullOrEmpty(row.Value) ? decimal.Parse(row.Value) : 0;
                    if (row.Key.ToLower() == "vote_count")
                        ratingsCount = !string.IsNullOrEmpty(row.Value) ? int.Parse(row.Value) : 0;
                    if (row.Key.ToLower() == "runtime")
                        runTime = !string.IsNullOrEmpty(row.Value) ? Convert.ToInt32(row.Value) : 0;
                }

                _logger.LogInformation($"Processando filme: {id} - {name}");

                bool titleExists = existingIds.Contains(id);

                if (titleExists)
                {
                    _logger.LogInformation($"Filme {id} - {name} já existe.");

                    continue;
                }

                Review review = new Review("IMDb", ratingAverage, ratingsCount)
                {
                    TitleExternalId = IMDbId
                };

                Movie movie = new Movie(name, description, backdrop_path, poster_path, new List<Review>() { review }, runTime)
                {
                    Id = id
                };
                titles.Add(movie);
            }

            await titleRepository.RegisterListAsync(titles);

            return Unit.Value;
        }
    }
}
