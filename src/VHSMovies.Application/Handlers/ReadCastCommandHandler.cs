using VHSMovies.Mediator;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Commands;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Mediator.Interfaces;
using VHSMovies.Mediator.Implementation;

namespace VHSMovies.Application.Handlers
{
    public class ReadCastCommandHandler : IRequestHandler<ReadCastCommand, Unit>
    {
        private readonly IPersonRepository personRepository;
        private readonly ITitleRepository<Title> titleRepository;
        private readonly ICastRepository castRepository;

        private readonly ILogger<ReadCastCommandHandler> _logger;

        public ReadCastCommandHandler(IPersonRepository personRepository,
            ILogger<ReadCastCommandHandler> _logger,
            ITitleRepository<Title> titleRepository,
            ICastRepository castRepository)
        {
            this.personRepository = personRepository;
            this._logger = _logger;
            this.titleRepository = titleRepository;
            this.castRepository = castRepository;
        }

        public async Task<Unit> Handle(ReadCastCommand command, CancellationToken cancellationToken)
        {
            List<Cast> newCasts = new List<Cast>();

            var existingPeople = new HashSet<Person>(await personRepository.GetAll());
            var existingTitles = new HashSet<Title>(await titleRepository.GetAll());

            List<string> validHeaders = new List<string>()
            {
                "personid",
                "name",
                "filmid",
                "departmentid"
            };

            foreach (var rows in command.CastRows)
            {
                int personId = 0;
                string name = "";
                int titleId = 0;
                PersonRole role = PersonRole.None;

                Person person = new Person();
                Cast cast = new Cast();

                bool matchKeys = validHeaders.All(header => rows.Any(r => r.Key.ToLower() == header));

                if (!matchKeys)
                    throw new KeyNotFoundException("Cabeçalhos não correspondentes.");

                foreach (var row in rows)
                {
                    if (row.Key.ToLower() == "personid")
                    {
                        personId = !string.IsNullOrEmpty(row.Value) ? Convert.ToInt32(row.Value) : 0;

                        if (personId == 0)
                            break;
                    }
                    if (row.Key.ToLower() == "name")
                        name = !string.IsNullOrEmpty(row.Value) ? row.Value : "";
                    if (row.Key.ToLower() == "filmid")
                        titleId = !string.IsNullOrEmpty(row.Value) ? Convert.ToInt32(row.Value) : 0;
                    if (row.Key.ToLower() == "departmentid")
                    {
                        int index = !string.IsNullOrEmpty(row.Value) ? Convert.ToInt32(row.Value) : 0;

                        if (!Enum.IsDefined(typeof(PersonRole), index))
                        {
                            role = PersonRole.None;
                            continue;
                        }

                        role = (PersonRole)Enum.ToObject(typeof(PersonRole), index);
                    }
                }

                if (role == PersonRole.None)
                    continue;

                _logger.LogInformation($"Processando pessoa do elenco: {personId} - {name} - {role.ToString()}");

                bool personExists = existingPeople.Any(x => x.Id == personId);

                bool titleExists = existingTitles.Any(x => x.Id == titleId);

                if (!titleExists)
                {
                    _logger.LogInformation($"Filme com Id {titleId} não existe.");

                    continue;
                }

                if (personExists && titleExists)
                {
                    if (newCasts.Any(x => x.TitleId == titleId && x.PersonId == personId))
                        continue;

                    _logger.LogInformation($"Pessoa \"{name}\" já existe.");

                    cast = new Cast()
                    {
                        PersonId = personId,
                        TitleId = titleId,
                        Role = role,
                    };

                    newCasts.Add(cast);

                    continue;
                }
            }

            if (newCasts.Any())
                await castRepository.RegisterListAsync(newCasts);

            return Unit.Value;
        }
    }
}
