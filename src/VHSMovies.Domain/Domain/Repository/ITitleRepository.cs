using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Domain.Domain.Repository
{
    public interface ITitleRepository
    {
        Task<List<Title>> GetAll();
        Task<List<Title>> GetAllByReviewerName(string reviewerName);
        Task<List<Title>> GetAllByGenreId(int genreId);
        Task<Title> GetByIdAsync(int id);
        Task<Title> GetByExternalIdAsync(string externalId);
        Task RegisterAsync(Title entity);
        Task RegisterListAsync(List<Title> entity);

        IQueryable<Title> Query();
        Task SaveChangesAsync();
    }
}
