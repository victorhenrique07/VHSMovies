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
    public class TitleRepository<T> : ITitleRepository<T> where T : Title
    {
        private readonly DbContextClass dbContextClass;

        public TitleRepository(DbContextClass dbContextClass)
        {
            this.dbContextClass = dbContextClass;
        }

        public async Task<IEnumerable<T>> GetAll(string reviewerName)
        {
            return await dbContextClass.Set<T>()
                .Where(t => t.Ratings.Select(r => r.Reviewer == reviewerName) != null)
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbContextClass.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> GetByExternalIdAsync(string externalId)
        {
            return await dbContextClass.Set<T>().FirstOrDefaultAsync(x => x.ExternalId == externalId);
        }

        public async Task UpdateAsync(T title)
        {
            dbContextClass.Set<T>().Update(title);
            await dbContextClass.SaveChangesAsync();
        }

        public async Task RegisterAsync(T entity)
        {
            await dbContextClass.Set<T>().AddAsync(entity);
            await dbContextClass.SaveChangesAsync();
        }
    }
}
