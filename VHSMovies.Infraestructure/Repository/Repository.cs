using LiveChat.Infraestructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Domain.Infraestructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContextClass _context;
        private readonly DbSet<T> dbContext;

        public Repository(DbContextClass context)
        {
            _context = context;
            dbContext = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAll(string reviewerName) => await dbContext.ToListAsync();

        public async Task<T> GetByIdAsync(int id) => await dbContext.FindAsync(id);

        public async Task<T> GetByExternalIdAsync(string externalId) => await dbContext.FindAsync(externalId);

        public async Task RegisterAsync(List<T> entity) => await dbContext.AddRangeAsync(entity);

        public async Task UpdateAsync(List<T> entity) => dbContext.UpdateRange(entity);

        public async Task SaveChanges() => _context.SaveChangesAsync();
    }
}
