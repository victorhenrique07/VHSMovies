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

namespace VHSMovies.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [RequestSizeLimit(100 * 1024 * 4096)]
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
                return BadRequest("Nenhum arquivo enviado ou arquivo vazio.");

            var stopwatch = Stopwatch.StartNew();

            List<Dictionary<string, string>> rows = await ReadCsvFile(file);

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
        public async Task<IActionResult> ReadReviews(IFormFile file, bool isCsv)
        {
            if (file == null || file.Length == 0)
                return BadRequest("None file sent or file is empty.");

            var stopwatch = Stopwatch.StartNew();

            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();

            if (isCsv)
            {
                rows = await ReadCsvFile(file);
            }
            else
            {
                rows = await ReadAllRows(file);
            }

            ReadReviewsCommand command = new ReadReviewsCommand()
            {
                ReviewsRows = rows,
                isCsv = isCsv
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

        private async Task<List<Dictionary<string, string>>> ReadCsvFile(IFormFile file)
        {
            var allData = new List<Dictionary<string, string>>();

            try
            {
                using var stream = file.OpenReadStream();
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    HeaderValidated = null,
                    Mode = CsvMode.RFC4180,
                    TrimOptions = TrimOptions.Trim,
                    Delimiter = ",",
                    PrepareHeaderForMatch = args => args.Header.Trim()
                });

                var records = new List<CsvMovie>();
                await foreach (var record in csv.GetRecordsAsync<CsvMovie>())
                {
                    if (record == null) continue;

                    var processedRecord = new Dictionary<string, string>
                    {
                        { "id", record.id },
                        { "title", record.title },
                        { "release_date", record.release_date },
                        { "runtime", record.runtime },
                        { "backdrop_path", record.backdrop_path },
                        { "tconst", record.tconst },
                        { "overview", record.overview },
                        { "poster_path", record.poster_path },
                        { "genres", record.genres },
                        { "averageRating", record.averageRating },
                        { "numVotes", record.numVotes }
                    };

                    allData.Add(processedRecord);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar o CSV: {ex.Message}");
                throw;
            }

            return allData;
        }
    }
}
