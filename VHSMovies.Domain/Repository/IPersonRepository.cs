using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Entity;

namespace VHSMovies.Domain.Repository
{
    public interface IPersonRepository
    {
        Task<T> GetPersonById<T>(int id) where T : Person;
        Task<T> GetPersonByExternalId<T>(string externalId) where T : Person;
        Task RegisterPerson<T>(T person) where T : Person;
    }
}
