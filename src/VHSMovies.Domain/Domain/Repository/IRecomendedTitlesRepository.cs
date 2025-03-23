using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Domain.Domain.Repository
{
    public interface IRecomendedTitlesRepository
    {
        Task<IReadOnlyCollection<RecommendedTitle>> GetAllRecommendedTitles(int titlesAmount);

        Task<RecommendedTitle> GetById(int id);

        IQueryable<RecommendedTitle> Query();
    }
}
