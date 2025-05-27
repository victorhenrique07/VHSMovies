using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using VHSMovies.Application.Commands;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Mediator;
using VHSMovies.Mediator.Implementation;
using VHSMovies.Mediator.Interfaces;

namespace VHSMovies.Application.Handlers
{
    public class ReadPeopleCommandHandler : IRequestHandler<ReadPeopleCommand, Unit>
    {
        private readonly IPersonRepository personRepository;

        private readonly ILogger<ReadPeopleCommandHandler> _logger;

        public ReadPeopleCommandHandler(IPersonRepository personRepository,
            ILogger<ReadPeopleCommandHandler> _logger)
        {
            this.personRepository = personRepository;
            this._logger = _logger;
        }

        public async Task<Unit> Handle(ReadPeopleCommand command, CancellationToken cancellationToken)
        {
            List<Person> newPeople = new List<Person>();

            var existingPeopleIds = new HashSet<int>((await personRepository.GetAllPerson()).Select(x => x.Id));

            HashSet<string> validHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "nconst",
                "primaryName",
                "birthYear",
                "deathYear"
            };

            foreach (var rowSet in command.PeopleRows)
            {
                var normalizedKeys = rowSet.Select(r => r.Key.Trim().ToLower()).ToHashSet();

                if (!validHeaders.IsSubsetOf(normalizedKeys))
                    throw new KeyNotFoundException("Headers do not match.");

                var values = rowSet.ToDictionary(
                    kv => kv.Key.Trim().ToLower(),
                    kv => kv.Value?.Trim() ?? string.Empty
                );

                string nconst = GetValueOrDefault(values["nconst"]);
                string primaryName = GetValueOrDefault(values["primaryname"]);
                int birthYear = ParseInt(values.GetValueOrDefault("birthyear"));
                int deathYear = ParseInt(values.GetValueOrDefault("deathyear"));

                _logger.LogInformation($"Processing person: {nconst} - {primaryName}");

                Person person = new Person(primaryName)
                {
                    IMDB_Id = nconst,
                    BirthYear = birthYear,
                    DeathYear = deathYear
                };

                newPeople.Add(person);
            }

            await personRepository.RegisterListAsync(newPeople);
            await personRepository.SaveChangesAsync();

            return Unit.Value;
        }

        private static string GetValueOrDefault(string? value) =>
            string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();

        private static int ParseInt(string value) =>
            int.TryParse(value, out var result) ? result : 0;
    }
}
