using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using VHSMovies.Application.Commands;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Mediator;
using VHSMovies.Mediator.Implementation;
using VHSMovies.Mediator.Interfaces;

namespace VHSMovies.Application.Handlers
{
    public class ReadCastCommandHandler : IRequestHandler<ReadCastCommand, Unit>
    {
        private readonly IPersonRepository personRepository;
        private readonly ITitleRepository titleRepository;
        private readonly ICastRepository castRepository;

        private readonly ILogger<ReadCastCommandHandler> _logger;

        public ReadCastCommandHandler(IPersonRepository personRepository,
            ILogger<ReadCastCommandHandler> _logger,
            ITitleRepository titleRepository,
            ICastRepository castRepository)
        {
            this._logger = _logger;
            this.personRepository = personRepository;
            this.titleRepository = titleRepository;
            this.castRepository = castRepository;
        }

        public async Task<Unit> Handle(ReadCastCommand command, CancellationToken cancellationToken)
        {
            var tconsts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var nconsts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var row in command.CastRows.SelectMany(r => r))
            {
                if (row.Key.Equals("tconst", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(row.Value))
                    tconsts.Add(row.Value.Trim());

                if (row.Key.Equals("nconst", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(row.Value))
                    nconsts.Add(row.Value.Trim());
            }

            var titleList = await titleRepository.Query()
                .Where(t => tconsts.Contains(t.IMDB_Id))
                .ToListAsync(cancellationToken);

            var personList = await personRepository.Query()
                .Where(n => nconsts.Contains(n.IMDB_Id))
                .ToListAsync(cancellationToken);

            var titleMap = titleList.ToDictionary(t => t.IMDB_Id, StringComparer.OrdinalIgnoreCase);
            var personMap = personList.ToDictionary(p => p.IMDB_Id, StringComparer.OrdinalIgnoreCase);

            var validRowSets = command.CastRows
                .Where(rowSet =>
                {
                    if (!rowSet.Any())
                        return false;

                    var values = rowSet.ToDictionary(
                        kv => kv.Key.Trim(),
                        kv => kv.Value?.Trim() ?? string.Empty,
                        StringComparer.OrdinalIgnoreCase
                    );

                    return values.TryGetValue("tconst", out var tconst) && !string.IsNullOrWhiteSpace(tconst) &&
                           titleMap.ContainsKey(tconst) && values.TryGetValue("nconst", out var nconst) &&
                           !string.IsNullOrWhiteSpace(nconst) && personMap.ContainsKey(nconst);
                })
                .ToList();

            command.CastRows.Clear();

            var newCasts = new List<Cast>();

            var existingCastKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var rowSet in validRowSets)
            {
                if (rowSet == null || rowSet.Count == 0)
                    continue;

                var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                foreach (var kv in rowSet)
                    values[kv.Key.Trim()] = kv.Value?.Trim() ?? string.Empty;

                if (!values.TryGetValue("tconst", out var tconst) || string.IsNullOrWhiteSpace(tconst) ||
                    !values.TryGetValue("nconst", out var nconst) || string.IsNullOrWhiteSpace(nconst) ||
                    !values.TryGetValue("category", out var category))
                {
                    _logger.LogWarning("Missing required cast data.");
                    continue;
                }

                var role = category.ToLowerInvariant() switch
                {
                    "actor" or "actress" => PersonRole.Actor,
                    "director" => PersonRole.Director,
                    "writer" => PersonRole.Writer,
                    _ => (PersonRole?)null
                };

                if (!role.HasValue)
                {
                    _logger.LogWarning($"Cast role '{category}' does not exist.");
                    continue;
                }

                if (!personMap.TryGetValue(nconst, out var person))
                {
                    _logger.LogWarning($"Person with nconst {nconst} not found.");
                    continue;
                }

                if (!titleMap.TryGetValue(tconst, out var title))
                {
                    _logger.LogWarning($"Title with tconst {tconst} not found.");
                    continue;
                }

                string castKey = $"{tconst}|{nconst}";

                if (!existingCastKeys.Add(castKey))
                    continue;

                _logger.LogInformation($"Processing Cast - Role: {role}; Person: {person.Name}; Title: {title.Name}");

                newCasts.Add(new Cast(title, person, role.Value));
            }

            validRowSets.Clear();

            await castRepository.RegisterListAsync(newCasts);
            await castRepository.SaveChanges();

            return Unit.Value;
        }

        private static string GetValueOrDefault(string? value) =>
            string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
    }
}
