using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Modules.Log;

using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using VHSMovies.Infraestructure;

namespace VHSMovies.Infraestructure.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DbContextClass dbContextClass;

        public PersonRepository(DbContextClass dbContextClass)
        {
            this.dbContextClass = dbContextClass;
        }

        public async Task<IReadOnlyCollection<Person>> GetAllPerson(PersonRole? personRole = null)
        {
            IReadOnlyCollection<Person> people = await dbContextClass.People
                .Include(p => p.Titles)
                    .ThenInclude(c => c.Title)
                .ToListAsync();

            if (personRole.HasValue)
            {
                people = people.Where(p => p.Titles.Any(r => r.Role == personRole)).ToList();
            }

            return people;
        }

        public async Task<Person> GetPersonById(int id)
        {
            Person? person = await dbContextClass.People
                .Where(p => p.Id == id).FirstOrDefaultAsync();

            if (person == null)
                throw new NotFoundException("Person Not Found");

            return person;
        }

        public IQueryable<Person> Query() => dbContextClass.People;

        public async Task RegisterListAsync(IReadOnlyCollection<Person> list) => await dbContextClass.People.AddRangeAsync(list);

        public async Task SaveChangesAsync() => await dbContextClass.SaveChangesAsync();
    }
}
