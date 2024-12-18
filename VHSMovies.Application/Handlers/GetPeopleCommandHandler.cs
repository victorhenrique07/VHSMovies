using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Commands;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Application.Handlers
{
    public class GetPeopleCommandHandler : IRequestHandler<GetPeopleCommand, IReadOnlyCollection<PersonResponse>>
    {
        private readonly IPersonRepository personRepository;

        public GetPeopleCommandHandler(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        public async Task<IReadOnlyCollection<PersonResponse>> Handle(GetPeopleCommand command, CancellationToken cancellationToken)
        {
            IEnumerable<Person> people = await personRepository.GetAllPerson(command.Role);
                
            var teste = people.Select(
                    person => new PersonResponse(
                        person.Id,
                        person.Name,
                        person.Titles.Select(
                            title => new TitleResponse(title.TitleId,title.Title.Name,title.Title.Description)
                            {
                                Genres = title.Title.Genres
                                    .Select(g => g.Genre.Name)
                                    .ToArray()
                            }
                        ).ToList()
                    )
                ).ToList();

            return teste;
        }
    }
}
