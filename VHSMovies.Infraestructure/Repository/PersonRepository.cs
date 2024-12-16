using LiveChat.Infraestructure;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium.BiDi.Modules.Log;
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

        public async Task RegisterAsync(List<Person> entity)
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

        public Task<IEnumerable<Person>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Person>> GetAllByReviewerName(string reviewerName)
        {
            throw new NotImplementedException();
        }
    }
}
