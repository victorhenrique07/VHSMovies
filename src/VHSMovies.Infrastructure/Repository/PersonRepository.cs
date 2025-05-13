using VHSMovies.Infraestructure;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium.BiDi.Modules.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using System.Text.Json;
using OpenQA.Selenium;

namespace VHSMovies.Infraestructure.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DbContextClass dbContextClass;

        public PersonRepository(DbContextClass dbContextClass)
        {
            this.dbContextClass = dbContextClass;
        }

        public async Task<IReadOnlyCollection<Person>> GetAllPerson(PersonRole personRole)
        {
            IReadOnlyCollection<Person> people = await dbContextClass.People
                .Include(p => p.Titles)
                    .ThenInclude(c => c.Title)
                .ToListAsync();

            if (personRole != PersonRole.None)
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

        public async Task RegisterListAsync(IReadOnlyCollection<Person> list) => await dbContextClass.People.AddRangeAsync(list);

        public async Task SaveChangesAsync() => await dbContextClass.SaveChangesAsync();
    }
}
