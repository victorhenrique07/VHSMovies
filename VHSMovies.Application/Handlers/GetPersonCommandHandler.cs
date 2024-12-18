using MediatR;
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
    public class GetPersonCommandHandler : IRequestHandler<GetPersonCommand, PersonResponse>
    {
        private readonly IPersonRepository personRepository;

        public GetPersonCommandHandler(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        public async Task<PersonResponse> Handle(GetPersonCommand command, CancellationToken cancellationToken)
        {
            Person person = await personRepository
                .GetByIdAsync(command.PersonId);

            List<TitleResponse> titles = person.Titles
                .Select(
                t => new TitleResponse(
                    t.TitleId, 
                    t.Title.Name, 
                    t.Title.Description))
                .ToList();

            return new PersonResponse(person.Id, person.Name, titles);
        }
    }
}
