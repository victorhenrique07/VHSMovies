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
    public class ReviewRepository : IReviewRepository
    {
        private readonly DbContextClass _dbContext;

        public ReviewRepository(DbContextClass dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Review>> GetAll()
        {
            return await _dbContext.Reviews.ToListAsync();
        }

        public async Task<List<Review>> GetByReviewerName(string reviewerName)
        {
            return await _dbContext.Reviews
                .Where(r => r.Reviewer.ToLower() == reviewerName)
                .ToListAsync();
        }

        public async Task<Review> GetByTitleExternalId(string titleExternalId)
        {
            return await _dbContext.Reviews
                .FirstOrDefaultAsync(r => r.TitleExternalId == titleExternalId);
        }

        public async Task UpdateReviews(List<Review> reviews)
        {
            try
            {
                _dbContext.Reviews.UpdateRange(reviews);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar dados: {ex.Message}");
                throw;
            }
        }
    }
}
