using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Domain.Domain.Repository
{
    public interface IRecommendedTitlesRepository
    {
        Task<IReadOnlyCollection<RecommendedTitle>> GetAllRecommendedTitles();

        Task<RecommendedTitle> GetById(int id);

        IQueryable<RecommendedTitle> Query();
    }
}
