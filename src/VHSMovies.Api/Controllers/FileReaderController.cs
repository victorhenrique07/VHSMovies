using System.Collections.Generic;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;

using CsvHelper;
using CsvHelper.Configuration;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.FileIO;

using OfficeOpenXml;

using VHSMovies.Application.Commands;
using VHSMovies.Application.Models;
using VHSMovies.Mediator;
using VHSMovies.Mediator.Interfaces;

namespace VHSMovies.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [RequestSizeLimit(2L * 1024 * 1024 * 1024 * 1024 * 1024)]
    public class FileReaderController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<FileReaderController> logger;

        public FileReaderController(IMediator mediator, ILogger<FileReaderController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpPost("read/titles")]
        public async Task<IActionResult> ReadTitlesCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("None file sent or file is empty.");

            var stopwatch = Stopwatch.StartNew();

            List<Dictionary<string, string>> rows = await ReadTitleCsvFile(file);

            ReadMoviesCommand command = new ReadMoviesCommand()
            {
                TitlesRows = rows,
                isCsv = true
            };

            await mediator.Send(command);

            stopwatch.Stop();

            return Ok(new
            {
                Status = StatusCode(201),
                Message = $"{rows.Count()} titles has been registered.",
                ProcessingTime = FormatTimeSpan(stopwatch.ElapsedMilliseconds)
            });
        }

        [HttpPost("read/people")]
        public async Task<IActionResult> ReadPeople(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("None file sent or file is empty.");

            var stopwatch = Stopwatch.StartNew();

            List<Dictionary<string, string>> rows = await ReadPersonCsvFile(file);

            ReadPeopleCommand command = new ReadPeopleCommand()
            {
                PeopleRows = rows
            };

            await mediator.Send(command);

            stopwatch.Stop();

            return Ok(new
            {
                Status = StatusCode(201),
                Message = $"{rows.Count()} people has been registered.",
                ProcessingTime = FormatTimeSpan(stopwatch.ElapsedMilliseconds)
            });
        }

        [HttpPost("read/cast")]
        public async Task<IActionResult> ReadCast(IFormFile castFile)
        {
            if (castFile == null || castFile.Length == 0)
                return BadRequest("None file sent or file is empty.");

            var stopwatch = Stopwatch.StartNew();

            List<Dictionary<string, string>> castRows = await ReadCastCsvFile(castFile);

            ReadCastCommand castCommand = new ReadCastCommand()
            {
                CastRows = castRows
            };

            await mediator.Send(castCommand);

            stopwatch.Stop();

            string timeEllapsed = FormatTimeSpan(stopwatch.ElapsedMilliseconds);

            logger.LogInformation($"{castRows.Count()} casts has been registered in {timeEllapsed}");

            return Ok(new
            {
                Status = StatusCode(201),
                Message = $"{castRows.Count()} casts has been registered.",
                ProcessingTime = FormatTimeSpan(stopwatch.ElapsedMilliseconds)
            });
        }

        [HttpPost("read/reviews")]
        public async Task<IActionResult> ReadReviews(IFormFile file, bool isCsv, string source)
        {
            if (file == null || file.Length == 0)
                return BadRequest("None file sent or file is empty.");

            var stopwatch = Stopwatch.StartNew();

            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();

            if (isCsv)
            {
                rows = await ReadReviewCsvFile(file);
            }

            ReadReviewsCommand command = new ReadReviewsCommand()
            {
                ReviewsRows = rows,
                isCsv = isCsv,
                Source = source
            };

            await mediator.Send(command);

            stopwatch.Stop();

            return Ok(new
            {
                Status = StatusCode(200),
                Message = $"{rows.Count()} reviews has been updated.",
                ProcessingTime = $"{stopwatch.Elapsed}"
            });
        }

        private async Task<List<Dictionary<string, string>>> ReadTitleCsvFile(IFormFile titlesFile)
        {
            var allData = new List<Dictionary<string, string>>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t",
                HasHeaderRecord = true,
                HeaderValidated = null,
                BadDataFound = null,
                MissingFieldFound = null,
                TrimOptions = TrimOptions.Trim,
                PrepareHeaderForMatch = args => args.Header.Trim()
            };

            using var stream = titlesFile.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, config);

            await foreach (var record in csv.GetRecordsAsync<CsvMovie>())
            {
                if (record == null) continue;

                if ((record.titleType.ToLower() != "tvseries" &&
                    record.titleType.ToLower() != "movie" &&
                    record.titleType.ToLower() != "tvmovie" &&
                    record.titleType.ToLower() != "tvminiseries") ||
                    record.tconst.IsNullOrEmpty())
                    continue;

                var processedRecord = new Dictionary<string, string>
                {
                    { "tconst", record.tconst },
                    { "titleType", record.titleType },
                    { "primaryTitle", record.primaryTitle },
                    { "startYear", record.startYear },
                    { "runtimeMinutes", record.runtimeMinutes },
                    { "genres", record.genres },
                };

                allData.Add(processedRecord);
            }

            return allData;
        }

        private async Task<List<Dictionary<string, string>>> ReadReviewCsvFile(IFormFile file)
        {
            var allData = new List<Dictionary<string, string>>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t",
                HasHeaderRecord = true,
                HeaderValidated = null,
                BadDataFound = null,
                MissingFieldFound = null,
                TrimOptions = TrimOptions.Trim,
                PrepareHeaderForMatch = args => args.Header.Trim()
            };

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, config);

            await foreach (var record in csv.GetRecordsAsync<CsvReview>())
            {
                if (record == null || record.tconst.IsNullOrEmpty()) continue;

                var processedRecord = new Dictionary<string, string>
                {
                    { "tconst", record.tconst },
                    { "averageRating", record.averageRating },
                    { "numVotes", record.numVotes },
                };

                allData.Add(processedRecord);
            }

            return allData;
        }

        private async Task<List<Dictionary<string, string>>> ReadPersonCsvFile(IFormFile file)
        {
            var allData = new List<Dictionary<string, string>>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t",
                HasHeaderRecord = true,
                HeaderValidated = null,
                BadDataFound = null,
                MissingFieldFound = null,
                TrimOptions = TrimOptions.Trim,
                PrepareHeaderForMatch = args => args.Header.Trim()
            };

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, config);

            await foreach (var record in csv.GetRecordsAsync<CsvPerson>())
            {
                if (record == null) continue;

                var processedRecord = new Dictionary<string, string>
                {
                    { "nconst", record.nconst },
                    { "primaryName", record.primaryName },
                    { "birthYear", record.birthYear },
                    { "deathYear", record.deathYear },
                };

                allData.Add(processedRecord);
            }

            return allData;
        }

        private async Task<List<Dictionary<string, string>>> ReadCastCsvFile(IFormFile file)
        {
            var allData = new List<Dictionary<string, string>>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t",
                HasHeaderRecord = true,
                HeaderValidated = null,
                BadDataFound = null,
                MissingFieldFound = null,
                TrimOptions = TrimOptions.Trim,
                PrepareHeaderForMatch = args => args.Header.Trim()
            };

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, config);

            await foreach (var record in csv.GetRecordsAsync<CsvCast>())
            {
                if (record == null) continue;

                if (record.category.ToLower() != "actor" &&
                    record.category.ToLower() != "actress" &&
                    record.category.ToLower() != "director" &&
                    record.category.ToLower() != "writer" ||
                    record.tconst.IsNullOrEmpty())
                    continue;

                var processedRecord = new Dictionary<string, string>
                {
                    { "tconst", record.tconst },
                    { "nconst", record.nconst },
                    { "category", record.category }
                };

                Console.WriteLine($"Title: {record.tconst}; Person: {record.nconst}");

                allData.Add(processedRecord);
            }

            return allData;
        }

        private string FormatTimeSpan(long ms)
        {
            int hours = (int)(ms / 3600000);
            int minutes = (int)((ms % 3600000) / 60000);
            int seconds = (int)((ms % 60000) / 1000);

            return $"{hours}h {minutes}m {seconds}s";
        }
    }
}
