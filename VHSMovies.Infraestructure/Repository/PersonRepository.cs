using LiveChat.Infraestructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Infraestructure.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DbContextClass dbContextClass;

        public PersonRepository(DbContextClass dbContextClass)
        {
            this.dbContextClass = dbContextClass;
        }

        public async Task<IEnumerable<Person>> GetAll(string reviewerName)
        {
            return await dbContextClass.People.Where(p => p.Name != null).ToListAsync();
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            return await dbContextClass.Set<Person>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Person> GetByExternalIdAsync(string externalId)
        {
            return await dbContextClass.Set<Person>().FirstOrDefaultAsync(x => x.ExternalId == externalId);
        }

        public async Task UpdateByExternalIdAsync(Person person)
        {
            dbContextClass.Set<Person>().Update(person);
            await dbContextClass.SaveChangesAsync();
        }

        public async Task RegisterAsync(Person entity)
        {
            await dbContextClass.Set<Person>().AddAsync(entity);
            await dbContextClass.SaveChangesAsync();
        }
    }
}
