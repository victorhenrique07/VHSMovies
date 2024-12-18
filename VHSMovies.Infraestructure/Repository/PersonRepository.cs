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

namespace VHSMovies.Infraestructure.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DbContextClass dbContextClass;

        public PersonRepository(DbContextClass dbContextClass)
        {
            this.dbContextClass = dbContextClass;
        }

        public async Task<IEnumerable<Person>> GetAllPerson(string personRole)
        {
            Enum.TryParse<PersonRole>(personRole, out PersonRole role);

            var teste = await dbContextClass.People
                .Include(p => p.Titles)
                    .ThenInclude(c => c.Title)
                .Where(p => p.Roles.Any(r => r.Role == role))
                .ToListAsync();

            return teste;
        }

        public async Task<bool> VerifyIfPersonExists(Person person)
        {
            try
            {
                Person existingPerson = await dbContextClass.People
                        .Where(p => p.ExternalId == person.ExternalId).FirstOrDefaultAsync();

                return existingPerson != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar pessoa: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Person>> GetAll()
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

        public async Task UpdateAsync(List<Person> people)
        {
            try
            {
                dbContextClass.Set<Person>().UpdateRange(people);
                await dbContextClass.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar dados: {ex.Message}");
                throw;
            }
        }

        public async Task RegisterAsync(Person entity)
        {
            try
            {
                await dbContextClass.Set<Person>().AddAsync(entity);
                await dbContextClass.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Erro ao salvar a entidade: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                    Console.WriteLine("Erro ao salvar: " + JsonSerializer.Serialize(entity));
                }
                throw;
            }
        }

        public async Task RegisterListAsync(List<Person> entity)
        {
            try
            {
                await dbContextClass.Set<Person>().AddRangeAsync(entity);
                await dbContextClass.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao registrar dados: {ex.Message}");
                throw;
            }
        }

        public async Task SaveChanges()
        {
            await dbContextClass.SaveChangesAsync();
        }

        public Task<IEnumerable<Person>> GetAllByReviewerName(string reviewerName)
        {
            throw new NotImplementedException();
        }
    }
}
