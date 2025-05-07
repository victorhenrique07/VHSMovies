using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Diagnostics;
using OfficeOpenXml;
using VHSMovies.Mediator;
using VHSMovies.Application.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using VHSMovies.Application.Commands;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using VHSMovies.Mediator.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace VHSMovies.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [RequestSizeLimit(2L * 1024 * 1024 * 1024)]
    public class FileReaderController : ControllerBase
    {
        private readonly IMediator mediator;

        public FileReaderController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("read/titles")]
        public async Task<IActionResult> ReadTitles(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("None file sent or file is empty.");

            var stopwatch = Stopwatch.StartNew();

            List<Dictionary<string, string>> rows = await ReadAllRows(file);

            ReadMoviesCommand command = new ReadMoviesCommand()
            {
                TitlesRows = rows
            };

            await mediator.Send(command);

            stopwatch.Stop();

            return Ok(new
            {
                Status = StatusCode(201),
                Message = $"{rows.Count()} titles has been registered.",
                ProcessingTime = $"{stopwatch.Elapsed} ms"
            });
        }

        [HttpPost("read/titles/csv")]
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
                ProcessingTime = $"{stopwatch.Elapsed} ms"
            });
        }

        [HttpPost("read/people")]
        public async Task<IActionResult> ReadPeople(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("None file sent or file is empty.");

            var stopwatch = Stopwatch.StartNew();

            List<Dictionary<string, string>> rows = await ReadAllRows(file);

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
                ProcessingTime = $"{stopwatch.Elapsed} ms"
            });
        }

        [HttpPost("read/cast")]
        public async Task<IActionResult> ReadCast(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("None file sent or file is empty.");

            var stopwatch = Stopwatch.StartNew();

            List<Dictionary<string, string>> rows = await ReadAllRows(file);

            ReadCastCommand command = new ReadCastCommand()
            {
                CastRows = rows
            };

            await mediator.Send(command);

            stopwatch.Stop();

            return Ok(new
            {
                Status = StatusCode(201),
                Message = $"{rows.Count()} casts has been registered.",
                ProcessingTime = $"{stopwatch.Elapsed} ms"
            });
        }

        [HttpPost("read/genres")]
        public async Task<IActionResult> ReadGenres(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("None file sent or file is empty.");

            var stopwatch = Stopwatch.StartNew();

            List<Dictionary<string, string>> rows = await ReadAllRows(file);

            ReadTitlesGenresCommand command = new ReadTitlesGenresCommand()
            {
                GenresRows = rows
            };

            await mediator.Send(command);

            stopwatch.Stop();

            return Ok(new
            {
                Status = StatusCode(201),
                Message = $"{rows.Count()} casts has been registered.",
                ProcessingTime = $"{stopwatch.Elapsed} ms"
            });
        }

        [HttpPatch("update/reviews")]
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

        private async Task<List<Dictionary<string, string>>> ReadAllRows(IFormFile file)
        {
            var rows = new List<Dictionary<string, string>>();

            using (var stream = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                await file.CopyToAsync(stream);
                stream.Position = 0;

                using (var package = new ExcelPackage(stream))
                {
                    foreach (var worksheet in package.Workbook.Worksheets)
                    {
                        var rowCount = worksheet.Dimension.Rows;
                        var colCount = worksheet.Dimension.Columns;

                        var headers = new List<string>();
                        for (int col = 1; col <= colCount; col++)
                        {
                            headers.Add(worksheet.Cells[1, col].Text);
                        }

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var rowData = new Dictionary<string, string>();
                            for (int col = 1; col <= colCount; col++)
                            {
                                rowData[headers[col - 1]] = worksheet.Cells[row, col].Text;
                            }
                            rows.Add(rowData);

                        }
                    }
                }
            }

            return rows;
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
    }
}
