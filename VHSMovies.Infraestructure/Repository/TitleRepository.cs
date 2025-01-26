using VHSMovies.Infraestructure;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium.BiDi.Modules.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;
using EFCore.BulkExtensions;

namespace VHSMovies.Infraestructure.Repository
{
    public class TitleRepository<T> : ITitleRepository<T> where T : Title
    {
        private readonly DbContextClass dbContextClass;

        public TitleRepository(DbContextClass dbContextClass)
        {
            this.dbContextClass = dbContextClass;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await dbContextClass.Set<T>()
                .Include(t => t.Genres)
                .ThenInclude(tg => tg.Genre)
                .Include(t => t.Ratings)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByReviewerName(string reviewerName)
        {
            return await dbContextClass.Set<T>()
                .Include(t => t.Ratings)
                .Where(t => t.Ratings.Any(r => r.Reviewer == reviewerName))
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbContextClass.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> GetByExternalIdAsync(string externalId)
        {
            return await dbContextClass.Set<T>().FirstOrDefaultAsync(x => x.Ratings.Any(e => e.TitleExternalId == externalId));
        }

        public async Task UpdateAsync(List<T> titles)
        {
            try
            {
                dbContextClass.Set<T>().UpdateRange(titles);
                await dbContextClass.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar dados: {ex.Message}");
                throw;
            }
        }

        public async Task RegisterAsync(T entity)
        {
            try
            {
                await dbContextClass.Set<T>().AddAsync(entity);

                Console.WriteLine($"Adding: {entity.ToString()}");

                await dbContextClass.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao registrar dados: {ex.Message}");
                throw;
            }
        }

        public async Task RegisterListAsync(List<T> entity)
        {
            try
            {
                await dbContextClass.Set<T>().AddRangeAsync(entity);
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
    }
}
