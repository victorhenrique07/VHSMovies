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

        public async Task UpdateAsync(List<Cast> casts)
        {
            try
            {
                _dbContext.Casts.UpdateRange(casts);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar Casts: {ex.Message}");
                throw;
            }
        }

        public async Task RegisterListAsync(List<Cast> casts)
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

        public async Task<Cast> GetCastForTitleAsync(int titleId, int personId)
        {

            var cast = await _dbContext.Casts
                .Where(c => c.TitleId == titleId && c.PersonId == personId)
                .FirstOrDefaultAsync();

            return cast;
        }

        public Task<IEnumerable<Cast>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Cast>> GetAllByReviewerName(string reviewerName)
        {
            throw new NotImplementedException();
        }

        public Task<Cast> GetByExternalIdAsync(string externalId)
        {
            throw new NotImplementedException();
        }

        public Task SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
