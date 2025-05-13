using VHSMovies.Infraestructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Infraestructure.Repository
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
        public async Task<IReadOnlyCollection<T>> GetAll() => await dbContext.ToListAsync();
        public async Task<T> GetByIdAsync(int id) => await dbContext.FindAsync(id);
        public async Task RegisterListAsync(IReadOnlyCollection<T> entity) => await dbContext.AddRangeAsync(entity);
        public async Task RegisterAsync(T entity) => await dbContext.AddAsync(entity);
        public async Task SaveChanges() => _context.SaveChangesAsync();
    }
}
