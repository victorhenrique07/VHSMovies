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
    public class RecommendedTitlesRepository : IRecommendedTitlesRepository
    {
        private readonly DbContextClass dbContextClass;

        public RecommendedTitlesRepository(DbContextClass dbContextClass)
        {
            this.dbContextClass = dbContextClass;
        }

        public async Task<IReadOnlyCollection<RecommendedTitle>> GetAllRecommendedTitles()
        {
            IQueryable<RecommendedTitle> titles = Query();

            return await titles
                .AsNoTracking()
                .OrderByDescending(t => t.Relevance)
                .ToListAsync();
        }

        public async Task<RecommendedTitle> GetById(int id)
        {
            return await dbContextClass.RecommendedTitles
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public IQueryable<RecommendedTitle> Query()
        {
            return dbContextClass.RecommendedTitles.AsNoTracking().AsQueryable();
        }
    }
}
