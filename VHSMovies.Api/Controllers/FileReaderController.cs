using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Diagnostics;
using OfficeOpenXml;
using MediatR;
using VHSMovies.Application.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using VHSMovies.Application.Commands;

namespace VHSMovies.Api.Controllers
{
    [ApiController]
    [Route("/api/read")]
    [RequestSizeLimit(100 * 1024 * 1024)]
    public class FileReaderController : ControllerBase
    {
        private readonly IMediator mediator;

        public FileReaderController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("titles")]
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

        [HttpPost("people")]
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

        [HttpPost("cast")]
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
    }
}
