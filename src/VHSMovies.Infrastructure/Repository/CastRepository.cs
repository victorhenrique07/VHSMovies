using VHSMovies.Infraestructure;
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
    public class CastRepository : ICastRepository
    {
        private readonly DbContextClass _dbContext;

        public CastRepository(DbContextClass dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Cast> GetByIdAsync(int id)
        {
            return await _dbContext.Casts.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task RegisterListAsync(IReadOnlyCollection<Cast> casts)
        {
            try
            {
                await _dbContext.Casts.AddRangeAsync(casts);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao registrar Casts: {ex.Message}");
                throw;
            }
        }

        public async Task RegisterAsync(Cast cast)
        {
            try
            {
                await _dbContext.Casts.AddAsync(cast);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao registrar Casts: {ex.Message}");
                throw;
            }
        }

        public async Task<IReadOnlyCollection<Cast>> GetAllCastByTitleAsync(int titleId)
        {

            var cast = await _dbContext.Casts
                .Where(c => c.TitleId == titleId)
                .ToListAsync();

            return cast;
        }

        public async Task<IReadOnlyCollection<Cast>> GetCastsByPersonRole(PersonRole role)
        {
            return await _dbContext.Casts
                .Where(c => c.Role == role).ToListAsync();
        }

        public async Task<IReadOnlyCollection<Cast>> GetAll()
        {
            return await _dbContext.Casts.ToListAsync();
        }

        public async Task SaveChanges() => await _dbContext.SaveChangesAsync();
    }
}
