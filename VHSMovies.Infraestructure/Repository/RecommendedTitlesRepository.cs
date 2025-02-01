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
    public class RecommendedTitlesRepository : IRecomendedTitlesRepository
    {
        private readonly DbContextClass dbContextClass;

        public RecommendedTitlesRepository(DbContextClass dbContextClass)
        {
            this.dbContextClass = dbContextClass;
        }

        public async Task<IReadOnlyCollection<RecommendedTitle>> GetAllRecommendedTitles()
        {
            return await dbContextClass.RecommendedTitles
                .OrderByDescending(t => t.Relevance)
                .ToListAsync();
        }

        public async Task<RecommendedTitle> GetById(int id)
        {
            return await dbContextClass.RecommendedTitles.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
