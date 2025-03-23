using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Commands;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Application.Handlers
{
    public class ReadPeopleCommandHandler : IRequestHandler<ReadPeopleCommand, Unit>
    {
        private readonly IPersonRepository personRepository;
        private readonly ITitleRepository<Title> titleRepository;
        private readonly ICastRepository castRepository;

        private readonly ILogger<ReadPeopleCommandHandler> _logger;

        public ReadPeopleCommandHandler(IPersonRepository personRepository,
            ILogger<ReadPeopleCommandHandler> _logger,
            ITitleRepository<Title> titleRepository,
            ICastRepository castRepository)
        {
            this.personRepository = personRepository;
            this._logger = _logger;
            this.titleRepository = titleRepository;
            this.castRepository = castRepository;
        }

        public async Task<Unit> Handle(ReadPeopleCommand command, CancellationToken cancellationToken)
        {
            List<Person> newPeople = new List<Person>();

            var existingPeopleIds = new HashSet<int>((await personRepository.GetAll()).Select(x => x.Id));

            List<string> validHeaders = new List<string>()
            {
                "personid",
                "name_"
            };

            foreach (var rows in command.PeopleRows)
            {
                int id = 0;
                string name = "";

                bool matchKeys = validHeaders.All(header => rows.Any(r => r.Key.ToLower() == header));

                if (!matchKeys)
                    throw new KeyNotFoundException("Cabeçalhos não correspondentes.");

                foreach (var row in rows)
                {
                    if (row.Key.ToLower() == "personid")
                        id = !string.IsNullOrEmpty(row.Value) ? Convert.ToInt32(row.Value) : 0;
                    if (row.Key.ToLower() == "name_")
                        name = !string.IsNullOrEmpty(row.Value) ? row.Value : "";
                }

                _logger.LogInformation($"Processando pessoa: {id} - {name}");

                bool personExists = existingPeopleIds.Contains(id);

                if (personExists)
                {
                    _logger.LogInformation($"Pessoa \"{name}\" já existe.");

                    continue;
                }

                Person person = new Person(name)
                {
                    Id = id
                };

                existingPeopleIds.Add(id);

                newPeople.Add(person);
            }

            await personRepository.RegisterListAsync(newPeople);

            return Unit.Value;
        }
    }
}
