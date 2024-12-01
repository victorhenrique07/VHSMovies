using LiveChat.Infraestructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Entity;
using VHSMovies.Domain.Repository;

namespace VHSMovies.Infraestructure.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DbContextClass _context;

        public PersonRepository(DbContextClass context)
        {
            _context = context;
        }

        public async Task<T> GetPersonById<T>(int id) where T : Person
        {
            return await _context.Set<T>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<T> GetPersonByExternalId<T>(string externalId) where T : Person
        {
            return await _context.Set<T>().Where(x => x.ExternalId == externalId).FirstOrDefaultAsync();
        }

        public async Task RegisterPerson<T>(T person) where T : Person
        {
            var entry = await _context.Set<T>().AddAsync(person);

            await _context.SaveChangesAsync();
        }
    }
}
