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
    public class ReviewRepository : IReviewRepository
    {
        private readonly DbContextClass dbContextClass;

        public ReviewRepository(DbContextClass dbContextClass)
        {
            this.dbContextClass = dbContextClass;
        }

        public async Task<List<Review>> GetAll()
        {
            return await dbContextClass.Reviews.ToListAsync();
        }

        public async Task<List<Review>> GetByReviewerName(string reviewerName)
        {
            return await dbContextClass.Reviews
                .Where(r => r.Reviewer.ToLower() == reviewerName.ToLower())
                .ToListAsync();
        }

        public async Task<Review> GetByTitleExternalId(string titleExternalId)
        {
            return await dbContextClass.Reviews
                .FirstOrDefaultAsync(r => r.TitleExternalId == titleExternalId);
        }

        public async Task AddReviews(List<Review> reviews)
        {
            try
            {
                await dbContextClass.Reviews.AddRangeAsync(reviews);
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException($"Error while adding reviews: {ex.Message}");
                
            }
        }

        public async Task SaveChangesAsync()
        {
            await dbContextClass.SaveChangesAsync();
        }
    }
}
