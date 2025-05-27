using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Domain.Domain.Repository
{
    public interface IPersonRepository
    {
        Task<IReadOnlyCollection<Person>> GetAllPerson(PersonRole? role = null);

        Task<Person> GetPersonById(int id);

        Task RegisterListAsync(IReadOnlyCollection<Person> list);

        IQueryable<Person> Query();

        Task SaveChangesAsync();
    }
}
